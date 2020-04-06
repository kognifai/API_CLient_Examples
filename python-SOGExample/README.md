SOG Example
----------

In order to run the associated python code. 
First a user needs to install the dependencies. These dependencies are listed in the requirements.txt file. Run the following command in the shell of the visual studio code. 

*pip install -r requirements.txt*

This project includes the following files. 

* Main.py: This is the entry point of the code. It has code for printing the final most recent timestamp received.
* Viauth.py: This file authticates based on the tenant, client id and client secret. This file also has the common header for both the API
User needs to update the all the variable marked *somevalue*
* Getfilterednode.py: This code will the get the timeserieid based on the nodetype and standard nodepath. 
User needs to update *shipname* in the nodePath variable.
* getlatestTS.py: This code will retrieve lastest speed over ground value based on timesereisid retrieved in getfilterednode.py. 
