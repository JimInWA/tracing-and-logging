- Assumptions:
	- You have Microsoft SQL Server installed on your Windows development machine
	- You have run the database script file 04_Database\DatabaseScripts.sql
		- This means that there is a SampleLogging database on your local SQL Server
		- Inside SampleLogging, there is the table dbo.SoapRequestAndResponseTracingBase 
	
- Open the 01_WCF_Service_SimpleMathService.sln file
	- Code for this service is in 01_WCF_Service\SimpleMathService

- Make sure that SimpleMathService is the Start Up Project

- Verify that the Web.config file for the SimpleMathService project has the appSettings key ShouldLogSoapRequestsAndResponses set to true

- RIGHT-click on AdditionOperations.svc and select View in Browser to run the site in non-debug (and so the WCF Test Client doesn't come up)

- The Proxy Class (Service Class) for the above WCF Service has been generated and is in 01a_ProxyClass folder

- Open the 02_ClientApplication_SimpleMathClient.sln file 
	- Code for this client is in 02_ClientApplication\SimpleMathClient

- Make sure that SimpleMathClient is the Start Up Project

- Verify settings in the Web.config file for the SimpleMathClient project
	- Has the appSettings key ShouldLogSoapRequestsAndResponses set to true
	- Scroll down to <client>
	- Make sure that endpoint name="AdditionOperations_BasicHttpBinding" is uncommented
		- This is the standard basic authentication which is the same as Soap11 with no addressing

- Run the site (non-debug) so the Client Application is up and running

- Notice that the addition is wrong, it should be 6
	- This error is there to simulate the basic troubleshooting process
	- In other words, what layer is the issue in - the UI layer or the Service layer
	- In this case the deliberate addition issue is in the Service layer
	
- There are 2 different places where the Soap Request and/or Response could be logged
	- The default functionality is to log to the database
	- If there are any exceptions logging to the database, the code is set log to the C:\Temp folder (configurable)
	
- Checking the database
	- Run the following script:
		- select * from [SampleLogging].dbo.SoapRequestAndResponseTracingBase order by ID

- Checking the filesystem
	- Browse to C:\Temp
	- You may see 2 files:
		- WCF_Server_Side_Log_File.log
		- MVC_Client_Side_Log_File.log
	- If you open the MVC Client file
		- If the request was logged, you can see that the client did in fact send 3 for both values so the response should have been 6
		- If the response was logged, you can see that the server sent back 5 as the response
	- If you open the WCF Server file
		- If the request was logged, you can see that the client did in fact send 3 for both values so the response should have been 6
		- If the response was logged, you can see that the server sent back 5 as the response

- Now you have enough information to fix the issue and what layer the issue is happening in

- When looking at the database
	- you may have noticed that the URN_UUID column has an empty GUID (all zeroes)
	- you may have noticed that in the SoapRequestOrResponseXml column
		- you may have noticed that the request entries don't have a MessageId in the header
		- you may have noticed that the response entries don't have a RelatesTo in the header

- If any of the files were logged to the filesystem
	- you may have noticed that the request files don't have a MessageId in the header
	- you may have noticed that the response files don't have a RelatesTo in the header

- For this very simplistic example, it is very easy to match up the requests and responses without the above values

- However, imagine a situation where you have hundreds of requests and responses to try to track and match up

- Re-open the Web.config file for the SimpleMathClient project
	- Scroll down to <client>
	- Comment out the endpoint name="AdditionOperations_BasicHttpBinding"
	- Uncomment out the endpoint name="AdditionOperations_CustomBinding"
		- This is the custom binding set up to mimic basic authentication with one major difference
			- It uses Soap11WSAddressing10, which will give you a MessageId on the request and a RelatesTo on the reply
	- Save the changes to Web.config
	
- Run the site (non-debug) so the Client Application is up and running

- Again, check the database and/or the filesystem for the log entries

- When looking at the database, this time you will see that the URN_UUID column has values 
	- you will notice that the same URN_UUID is being tracked for the Client and Server request and responses
		- This is correct as it lets you coordinate log requests across multiple systems
		- In a production system, it is assumed that the client would be logging to one database and the server would be logging to another database
	- you will notice that the request entries do have a MessageId in the header
	- you will notice that the response entries do have a RelatesTo in the header
		
- When looking at the filesystem
	- you will notice that the request files do have a MessageId in the header
	- you will notice that the response files do have a RelatesTo in the header
