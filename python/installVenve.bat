python -m venv astro_env
astro_env\scripts\python.exe -m pip install --upgrade pip
set arg=%1
set arg=%arg:"=%
astro_env\scripts\python.exe -m pip install -r "%arg%requirements.txt"
