import requests, json 
from urllib.parse import quote 
import getfilterednode 
import viauth


timeseriesId = getfilterednode.timeseriesID
url = 'https://api.kognif.ai/assets/v2-preview/Timeseries/LatestValue/{}'.format(quote(timeseriesId))

r = requests.get(url, headers=viauth.headers) # API Request

