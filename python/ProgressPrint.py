# WIP pune intended

class progress():
    def __init__(self,module_name:str, stages:int ):
        self.module_name = module_name
        self.stages = stages
        self.on_stage: int = 0
        
    def print(self, massage:str, stage:int):
        print('{"module_name":\"{self.module_name}\","massage":\"{massage}\","progress":{stage/self.stages}}')

