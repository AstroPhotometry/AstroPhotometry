# Astro Photometry
## packages:
- astropy
- Numpy

## venv installation
```shell
$ python -m venv --system-site-packages .\venv
```
to start venv type:   
```shell
$ &"./venv/Scripts/Activate.ps1"
```
$ to get out of venv type:
```shell
$ deactivate
```
if error in stating, type in admin cmd:
```shell
$ Set-ExecutionPolicy Unrestricted -Scope Process
```

## packages
update:
```shell
$ python -m pip install --upgrade pip
```
astropy:
```shell
$ python -m pip install --upgrade astropy[recommended]
```
pillow:
```shell
$ python3 -m pip install --upgrade Pillow
```
matplotlib:
```shell
$ pip install matplotlib
```
if its not installing type without the `python -m `

## reference
Structure of FITS files: http://www.eso.org/sci/software/esomidas/doc/user/18NOV/vola/node111.html
FITS File Handling with astropy in python: https://docs.astropy.org/en/stable/io/fits/index.html
