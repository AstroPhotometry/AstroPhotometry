import json


class JsonConvert:
    def __init__(self, file_path):
        self.file_path = file_path

    def load_data(self):
        data = ''
        with open(self.file_path, 'r') as json_obj:
            data = json.load(json_obj)

        return data['fitsToPNG'], data['bias'], data['dark'], data['flat'], data['light'], data['outputMasterBias'], \
               data['outputMasterDark'], data['outputMasterFlat'], data['outputCallibrationFile'], \
               data['outputCallibratedFolder']



if __name__ == '__main__':
    with open('./stam.json', 'r') as json_obj:
       a = json.load(json_obj)
       print(a)