##################################################################
# BEFORE EXECUTING MAKEFILE:
# In Get_Data / Rebill / Transactions / Sample directory:
#    - Insert required values in the sample file you'd like to run
# In bluepay-cpp/main.cpp:
#    - Uncomment the sample file you'd like to run
#    - Uncomment the function you'd like to run
# In Makefile:
#    - Uncomment the path to the sample file you'd like to run
#    - Update any settings that are particular to your system
##################################################################

##################################################################
# Uncomment the path to the sample file you'd like to run
##################################################################
# COMPLETE_SAMPLE_PATH = Get_Data/Retrieve_Settlement_Data
# COMPLETE_SAMPLE_PATH = Get_Data/Retrieve_Transaction_Data
# COMPLETE_SAMPLE_PATH = Get_Data/Single_Transaction_Query

# COMPLETE_SAMPLE_PATH = Rebill/Cancel_Recurring_Payment
# COMPLETE_SAMPLE_PATH = Rebill/Create_Recurring_Payment_ACH
# COMPLETE_SAMPLE_PATH = Rebill/Create_Recurring_Payment_CC
# COMPLETE_SAMPLE_PATH = Rebill/Get_Recurring_Payment_Status
# COMPLETE_SAMPLE_PATH = Rebill/Update_Recurring_Payment

# COMPLETE_SAMPLE_PATH = Transactions/Cancel_Transaction
# COMPLETE_SAMPLE_PATH = Transactions/Charge_Customer_ACH
# COMPLETE_SAMPLE_PATH = Transactions/Charge_Customer_CC
# COMPLETE_SAMPLE_PATH = Transactions/Check_Customer_Credit
# COMPLETE_SAMPLE_PATH = Transactions/Customer_Defined_Data
# COMPLETE_SAMPLE_PATH = Transactions/How_To_Use_Token
# COMPLETE_SAMPLE_PATH = Transactions/Return_Funds
# COMPLETE_SAMPLE_PATH = Transactions/Store_Payment_Information
# COMPLETE_SAMPLE_PATH = Transactions/Swipe

##################################################################
# Settings particular to your system
##################################################################
# LIB_PATH must point to the directory where you have libcurl
# Generally something like "-L/usr/local/lib"
LIB_PATH = 
LIB = -lcurl

##################################################################
# VARIABLES
##################################################################
# Program Settings
CXX = g++
DEBUG = -g
CXXFLAGS = -Wall -c $(DEBUG)
LFLAGS = -Wall $(DEBUG)
VPATH = $(BP_PATH) $(SAMPLE_PATH)
BP_OBJ = main.o BluePay.o Sha512.o md5.o
BP_DEP = BluePay.cpp BluePay.h Sha512.h md5.h curl.h curlver.h curlbuild.h curlrules.h
BP_DEP_FILE = $(BP_PATH)/Sha512.cpp $(BP_PATH)/md5.cpp
BP_PATH = bluepay-cpp

# Sample Specific Settings
SAMPLE = $(notdir $(COMPLETE_SAMPLE_PATH))
SAMPLE_SRC = $(SAMPLE).cpp
SAMPLE_OBJ = $(SAMPLE).o
SAMPLE_DEP = $(SAMPLE_SRC) $(SAMPLE).h BluePay.h
SAMPLE_PATH = $(dir $(COMPLETE_SAMPLE_PATH))

##################################################################
# PROGRAM
##################################################################
run_$(SAMPLE) : $(BP_OBJ) $(SAMPLE_OBJ)
	$(CXX) $(LFLAGS) main.o $(SAMPLE_OBJ) BluePay.o $(LIB_PATH) $(LIB) $(BP_DEP_FILE) -o $@

main.o : main.cpp BluePay.h $(SAMPLE_PATH)/$(SAMPLE).h
	$(CXX) $(CXXFLAGS) $(BP_PATH)/main.cpp 

BluePay.o : $(BP_DEP)
	$(CXX) $(CXXFLAGS) $(BP_PATH)/BluePay.cpp

Sha512.o : Sha512.cpp Sha512.h
	$(CXX) $(CXXFLAGS) $(BP_PATH)/Sha512.cpp

md5.o : md5.cpp md5.h
	$(CXX) $(CXXFLAGS) $(BP_PATH)/md5.cpp

$(SAMPLE_OBJ) : $(SAMPLE_DEP)
	$(CXX) $(CXXFLAGS) $(SAMPLE_PATH)/$(SAMPLE_SRC)


##################################################################
# REMOVE TARGETS & EXECUTIONABLES 
##################################################################
clean :
	\rm *.o run_*

.PHONY : clean 
