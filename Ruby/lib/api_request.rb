class BluePay
  
  # Turns a hash into a nvp style string
  def uri_query(param_hash)
    array = []
    param_hash.each_pair {|key, val| array << (URI.escape(key) + "=" + URI.escape(val))}
    uri_query_string = array.join("&")
    return uri_query_string
  end

  # Generates TPS hash based on given hash type
  def create_tps_hash(data, hash_type)
    case hash_type   
    when 'HMAC_SHA256'
      OpenSSL::HMAC.hexdigest('sha256', @SECRET_KEY, data)
    when 'SHA512'
      Digest::SHA512.hexdigest(@SECRET_KEY + data)
    when 'SHA256'
      Digest::SHA256.hexdigest(@SECRET_KEY + data)
    when 'MD5'
      Digest::MD5.hexdigest(@SECRET_KEY + data)
    else
      OpenSSL::HMAC.hexdigest('sha512', @SECRET_KEY, data)
    end
  end

  # Sets TAMPER_PROOF_SEAL in @PARAM_HASH
  def calc_tps
    @PARAM_HASH["TAMPER_PROOF_SEAL"] = create_tps_hash(
        @ACCOUNT_ID + 
        (@PARAM_HASH["TRANSACTION_TYPE"] || '') + 
        @PARAM_HASH["AMOUNT"] + 
        (@PARAM_HASH["REBILLING"] || '') + 
        (@PARAM_HASH["REB_FIRST_DATE"] || '') + 
        (@PARAM_HASH["REB_EXPR"] || '') + 
        (@PARAM_HASH["REB_CYCLES"] || '') + 
        (@PARAM_HASH["REB_AMOUNT"] || '') + 
        (@PARAM_HASH["RRNO"] || '') + 
        @PARAM_HASH["MODE"], 
        @PARAM_HASH['TPS_HASH_TYPE']
      )
  end

  # Sets TAMPER_PROOF_SEAL in @PARAM_HASH for rebadmin API
  def calc_rebill_tps
    @PARAM_HASH["TAMPER_PROOF_SEAL"] = create_tps_hash(
      @ACCOUNT_ID +
      @PARAM_HASH["TRANS_TYPE"] + 
      @PARAM_HASH["REBILL_ID"], 
      @PARAM_HASH['TPS_HASH_TYPE']
    )
  end

  # Sets TAMPER_PROOF_SEAL in @PARAM_HASH for bpdailyreport2 API
  def calc_report_tps
    @PARAM_HASH["TAMPER_PROOF_SEAL"] = create_tps_hash(
      @ACCOUNT_ID + 
      @PARAM_HASH["REPORT_START_DATE"] + 
      @PARAM_HASH["REPORT_END_DATE"],
      @PARAM_HASH['TPS_HASH_TYPE']
      )
  end

  # Calculates TAMPER_PROOF_SEAL to be used with Trans Notify API 
  def self.calc_trans_notify_tps(secret_key, trans_id, trans_status, trans_type, amount, batch_id, batch_status, total_count, total_amount, batch_upload_id, rebill_id, rebill_amount, rebill_status, hash_type)
    create_tps_hash(
      secret_key + 
      trans_id + 
      trans_status + 
      transtype + 
      amount + 
      batch_id + 
      batch_status + 
      total_count + 
      total_amount + 
      batch_upload_id + 
      rebill_id + 
      rebill_amount + 
      rebill_status, 
      hash_type
    )
  end

 # sends HTTPS POST to BluePay gateway for processing
  def process
    
    ua = Net::HTTP.new(SERVER, 443)
    ua.use_ssl = true
    
    # Set default hash function to HMAC SHA-512
    @PARAM_HASH['TPS_HASH_TYPE'] = 'HMAC_SHA512'

    # Checks presence of CA certificate
    if File.directory?(RootCA)
      ua.ca_path = RootCA
      ua.verify_mode = OpenSSL::SSL::VERIFY_PEER
      ua.verify_depth = 3
    else
      puts "Invalid CA certificates directory. Exiting..."
      exit
    end
    
    # Sets REMOTE_IP parameter
    begin
    	@PARAM_HASH["REMOTE_IP"] = request.env['REMOTE_ADDR']
      rescue Exception
    end

    # Generate the query string and headers.  Chooses which API to make request to.
    case @api
    when "bpdailyreport2"
      calc_report_tps
      path = "/interfaces/bpdailyreport2"
      query = "ACCOUNT_ID=#{@ACCOUNT_ID}&" + uri_query(@PARAM_HASH)
    when "stq"
      calc_report_tps
      path = "/interfaces/stq"
      query = "ACCOUNT_ID=#{@ACCOUNT_ID}&" + uri_query(@PARAM_HASH)
    when "bp10emu"
      calc_tps
      path = "/interfaces/bp10emu"
      query = "MERCHANT=#{@ACCOUNT_ID}&" + uri_query(@PARAM_HASH)
      # puts "****"; puts uri_query(@PARAM_HASH).inspect
    when "bp20rebadmin"
      calc_rebill_tps
      path = "/interfaces/bp20rebadmin"
      query = "ACCOUNT_ID=#{@ACCOUNT_ID}&" + uri_query(@PARAM_HASH)
    end
    queryheaders = {
      'User-Agent' => 'Bluepay Ruby Client',
      'Content-Type' => 'application/x-www-form-urlencoded'
    }
    # Response version to be returned
    @PARAM_HASH["VERSION"] = '3'
    # Post parameters to BluePay gateway
    headers, body = ua.post(path, query, queryheaders)
    # Split the response into the response hash.
    @RESPONSE_HASH = {}
    if path == "/interfaces/bp10emu"
      response = headers["Location"].split("?")[1]
    else
      response = headers.body
    end
    if path == "/interfaces/bpdailyreport2"
      response
    else
      response.split("&").each do |pair| 
        (key, val) = pair.split("=")
        val = "" if val == nil
        @RESPONSE_HASH[URI.unescape(key)] = URI.unescape(val) 
      end
    end
  end
end