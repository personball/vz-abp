vz project templates

# backend
abp.io

## microservice:

BaseOn:

``` bash
abp new CompanyName.ProjectName -dbms PostgreSQL --tiered --theme basic -csf
abp new CompanyName.ProjectName.ModuleName -t module --no-ui 
```

## Usage

``` bash
git clone https://github.com/personball/vz-abp.git
cd vz-abp

dotnet tool install vz-generator -g
vz rn CompanyName.ProjectName -r CompanyName=VZero -r ProjectName=Demo -o .
vz rn CompanyName.ProjectName.ModuleName -r CompanyName=VZero -r ProjectName=Demo -r ModuleName=IM -o .
```
