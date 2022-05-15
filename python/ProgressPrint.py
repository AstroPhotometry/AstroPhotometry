# WIP pune intended

import json


class Progress():

    def __init__(self, module_name: str, stages: int):
        self.module_name = module_name
        self.stages = stages
        self.on_stage: int = 0

    def cprint(self, message: str):
        self.print(message, self.on_stage)
        self.on_stage += 1

    def eprint(self, message: str):
        message = "ERROR: " + message
        self.print(message, self.stages)

    def print(self, message: str, stage: int):
        data_set = {
            "module_name": self.module_name,
            "message": message,
            "progress": round((stage / self.stages) * 100, 2)
        }

        if (stage < 0 or stage > self.stages):
            data_set["progress"] = -1

        json_dump = json.dumps(data_set)
        print(json_dump, flush=True)


if __name__ == "__main__":
    test = Progress("me", 5)
    test.cprint("testing fase")
    test.cprint("testing fase")
