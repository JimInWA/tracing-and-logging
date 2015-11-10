1. Open the 01_WCF_Service_SimpleMathService.sln file
- Code for this service is in 01_WCF_Service\SimpleMathService

2. Make sure that SimpleMathService is the Start Up Project

3. Verify that the Web.config file for the SimpleMathService project has the appSettings key ShouldLogSoapRequestsAndResponses set to true

4. Run the site (non-debug) so the WCF Service is up and running

5. The Proxy Class (Service Class) for the above WCF Service has been generated and is in 01a_ProxyClass folder

6. Open the 02_ClientApplication_SimpleMathClient.sln file 
- Code for this client is in 02_ClientApplication\SimpleMathClient

7. Make sure that SimpleMathClient is the Start Up Project

8. Verify that the Web.config file for the SimpleMathClient project has the appSettings key ShouldLogSoapRequestsAndResponses set to true

9. Run the site (non-debug) so the Client Application is up and running

10. Notice that there is an error in the addition - this is to simulate the basic troubleshooting process; in other words, what layer is the issue in - the UI layer or the Service layer

11. Browse to C:\Temp

12. You should see 2 files:
- WCF_Server_Side_Log_File.log
- MVC_Client_Side_Log_File.log

13. If you open the MVC Client file, you can see that the client did in fact send 3 for both values so the response should have been 6, but the server sent back 5 as the response

14. If you open the WCF Server file, you can see again that the client did in fact send 3 for both values so the response should have been 6, but the server sent back 5 as the response

15. Now you have enough information to fix the issue and what layer the issue is happening in

