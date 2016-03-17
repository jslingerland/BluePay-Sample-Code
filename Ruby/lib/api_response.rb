class BluePay
  def get_response
    @RESPONSE_HASH
  end

  # Returns true if response status is approved and not a duplicate, else returns false
  def successful_transaction?
    self.get_status == "APPROVED" && self.get_message != "DUPLICATE"
  end

  # Returns E for Error, 1 for Approved, 0 for Decline
  def get_status
    @RESPONSE_HASH['Result']
  end

  # Returns the human-readable response from Bluepay.
  # Or a nasty error.
  def get_message
    m = @RESPONSE_HASH['MESSAGE']
    if m == nil or m == ""
      "ERROR - NO MESSAGE FROM BLUEPAY"
    else
      m
    end
  end

  # Returns the single-character AVS response from the 
  # Card Issuing Bank
  def get_avs_code
    @RESPONSE_HASH['AVS']
  end

  # Same as avs_code, but for CVV2
  def get_cvv2_code
    @RESPONSE_HASH['CVV2']
  end

  # In the case of an approved transaction, contains the
  # 6-character authorization code from the processing network.
  # In the case of a decline or error, the contents may be junk.
  def get_auth_code
    @RESPONSE_HASH['AUTH_CODE']
  end

  # The all-important transaction ID.
  def get_trans_id
    @RESPONSE_HASH['RRNO']
  end

  # If you set up a rebilling, this'll get its ID.
  def get_rebill_id
    @RESPONSE_HASH['REBID']
  end

  # Masked credit card or ACH account
  def get_masked_account
    @RESPONSE_HASH['PAYMENT_ACCOUNT']
  end

  # Card type used in transaction
  def get_card_type
    @RESPONSE_HASH['CARD_TYPE']
  end

  # Bank account used in transaction
  def get_bank_name
    @RESPONSE_HASH['BANK_NAME']
  end

  # Rebill ID from bprebadmin API
  def get_reb_id
    @RESPONSE_HASH['rebill_id']
  end

  # Template ID of rebilling
  def get_template_id
    @RESPONSE_HASH['template_id']
  end

  # Status of rebilling
  def get_rebill_status
    @RESPONSE_HASH['status']
  end

  # Creation date of rebilling
  def get_creation_date
    @RESPONSE_HASH['creation_date']
  end
  
  # Next date that the rebilling is set to fire off on
  def get_next_date
    @RESPONSE_HASH['next_date']
  end

  # Last date that the rebilling fired off on
  def get_last_date
    @RESPONSE_HASH['last_date']
  end

  # Rebilling expression
  def get_sched_expression
    @RESPONSE_HASH['sched_expr']
  end
  
  # Number of cycles remaining on rebilling
  def get_cycles_remaining
    @RESPONSE_HASH['cycles_remain']
  end

  # Amount to charge when rebilling fires off
  def get_rebill_amount
    @RESPONSE_HASH['reb_amount']
  end

  # Next amount to charge when rebilling fires off
  def get_next_amount
    @RESPONSE_HASH['next_amount']
  end
 
  # Transaction ID used with stq API
  def get_id
    @RESPONSE_HASH['id']
  end 

  # First name associated with the transaction
  def get_name1
    @RESPONSE_HASH['name1']
  end
 
  # Last name associated with the transaction
  def get_name2
    @RESPONSE_HASH['name2']
  end
  
  # Payment type associated with the transaction
  def get_payment_type
    @RESPONSE_HASH['payment_type']
  end

  # Transaction type associated with the transaction
  def get_trans_type
    @RESPONSE_HASH['trans_type']
  end
  
  # Amount associated with the transaction
  def get_amount
    @RESPONSE_HASH['amount']
  end

end