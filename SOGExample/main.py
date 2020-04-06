import viauth 
import json
import getlatestTS as lts

def jprint(obj): #Jsonify
    text = json.dumps(obj, sort_keys=True, indent=4)
    print(text)

if __name__ == "__main__":
    jprint(lts.r.json())