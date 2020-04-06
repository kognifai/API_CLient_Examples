import adal

tenant = '<Insert tenant id>'
client_id = '<Insert Client-ID>'
client_secret = '<Insert client secret>'

authority_url = 'https://login.microsoftonline.com/' + tenant


resource = '<Insert Kognifai Resource ID>'  
context = adal.AuthenticationContext(authority_url) # Authentication 
token = context.acquire_token_with_client_credentials(resource, client_id, client_secret) # getting the token

headers = {
    'Cache-Control': 'no-cache',
    'Ocp-Apim-Subscription-Key': '<Insert API key from API portal>',
    'authorization' : 'Bearer '+token['accessToken']

} # Header for the API