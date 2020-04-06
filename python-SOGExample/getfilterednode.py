import requests, json 
from urllib.parse import quote 
import viauth

nodeType = 'TimeSeries'  # Based on the type of nodes available in galore
maxLevelOfEdges='1'     #Level of tree traversal, max 2
nodePath = '/Fleet/<VesselName>/Bridge/Navigation/SOG'
url = 'https://api.kognif.ai/assets/v2-preview/assetmodel/nodes?nodeType={}&path={}&maxLevelOfEdges={}'.format(quote(nodeType),quote(nodePath),quote(maxLevelOfEdges))
r = requests.get(url, headers=viauth.headers) # API request based on the URL

timeseriesID = r.json()[0]['timeseriesId'] # Getting the timesseried id value using slicing.