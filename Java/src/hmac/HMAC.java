package hmac;
import java.util.stream.IntStream;
import java.security.MessageDigest;

public class HMAC {  

	private String SECRET_KEY = "";
	private String MESSAGE = "";
	private String HASH_TYPE = "";
	private Integer BLOCK_SIZE = 64;
	private int[] I_PAD;
	private int[] O_PAD;

	public HMAC(String secretKey, String message, String hashType) {
		SECRET_KEY = secretKey;
		MESSAGE = message;
		HASH_TYPE = hashType;
		if (HASH_TYPE == "SHA-512") {
			BLOCK_SIZE = 128;
		}
		adjustKeyLength();
		xOrPads();
	}
	
	private void adjustKeyLength() {
		if(SECRET_KEY.length() > BLOCK_SIZE) {
			SECRET_KEY = getHash(SECRET_KEY.getBytes());
		} else if (SECRET_KEY.length() < BLOCK_SIZE) {
			IntStream.range(SECRET_KEY.length(), (BLOCK_SIZE) ).forEach( $ -> SECRET_KEY += (char)0x00 );
		}
	}
	
	private String getHash(byte[] inputBytes) {
		StringBuffer hashValue = new StringBuffer();
		try {
			MessageDigest mDigest = MessageDigest.getInstance(HASH_TYPE);
			mDigest.update(inputBytes);
			byte[] digest = mDigest.digest();
			for(byte b : digest) {
				String k = Integer.toHexString(b & 0xff);
				k = ("00" + k).substring(k.length());
				hashValue.append(k);
			}
		}
		catch(Exception e) {
			
		}
		return hashValue.toString().toUpperCase();
	}
	
	private void xOrPads() {
		I_PAD = new int[BLOCK_SIZE];
		O_PAD = new int[BLOCK_SIZE];
		
		byte[] key = SECRET_KEY.getBytes();
		
		for( int i = 0; i < BLOCK_SIZE; i++ ) {
			I_PAD[i] = (char)((byte)0x36 ^ key[i]);
			O_PAD[i] = (char)((byte)0x5c ^ key[i]);
		}
	}

	public String getHMAC() {
		StringBuffer result = new StringBuffer();
		
		try {
			// Concatenate byte arrays of I_PAD & MESSAGE
			byte[] messageBytes = MESSAGE.getBytes();
			byte[] innerInputBytes = new byte[BLOCK_SIZE + MESSAGE.length()];
			for (int i = 0; i < innerInputBytes.length; i++ ) {
				innerInputBytes[i] = i < BLOCK_SIZE ? (byte)I_PAD[i] : messageBytes[i - BLOCK_SIZE];
			}
			
			// Hash inner concatenation
			MessageDigest iDigest = MessageDigest.getInstance(HASH_TYPE);
			iDigest.update(innerInputBytes);
			byte[] innerDigest = iDigest.digest();
						
			// Concatenate O_PAD with result from previous step
			byte[] outerInputBytes = new byte[BLOCK_SIZE + innerDigest.length];
			for (int i = 0; i < outerInputBytes.length; i++) {
				outerInputBytes[i] = i < BLOCK_SIZE ? (byte)O_PAD[i] : innerDigest[i - BLOCK_SIZE];
			}
			
			// Hash inner concatenation
			MessageDigest oDigest = MessageDigest.getInstance(HASH_TYPE);
			oDigest.update(outerInputBytes);
			byte[] digest = oDigest.digest();

			// Convert to Hex
			for(byte b : digest) {
				String k = Integer.toHexString(b & 0xff);
				k = ("00" + k).substring(k.length());
				result.append(k);
			}
		}
		catch(Exception e) {
			
		}
		
		return result.toString().toUpperCase();
	}
}