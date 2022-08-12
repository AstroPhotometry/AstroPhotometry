import json


class JsonConvert:
    def __init__(self, file_path):
        self.file_path = file_path

    def load_data(self):
        with open(self.file_path, 'r', encoding='utf-8') as json_obj:
            data = json.load(json_obj)

        return data['fitsToPNG'], data['bias'], data['dark'], data['flat'], data['light'], data['outputMasterBias'], \
               data['outputMasterDark'], data['outputMasterFlat'], data['outputCallibrationFile'], \
               data['outputCallibratedFolder'], data['solve_stars_plate']
