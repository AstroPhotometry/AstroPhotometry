import json


class JsonConvert:
    def __init__(self, json_obj):
        self.json_obj = json_obj

    def run(self):
        data = json.loads(self.json_obj)
        return data['fitsToPNG'], data['bias'], data['dark'], data['flats'], data['light'], data['outputMasterBias'], \
               data['outputMasterDark'], data['outputMasterFlat'], data['outputCallibrationFile'], data[
                   'outputCallibratedFolder']
