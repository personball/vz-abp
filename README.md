vz project templates

# backend
abp.io

## microservice:

BaseOn:

```
abp new CompanyName.ProjectName -dbms PostgreSQL --tiered --theme basic -csf
abp new CompanyName.ProjectName.ModuleName -t module --no-ui 
```

## Usage

```
dotnet tool install vz-generator -g

cd vz-abp
vz rn CompanyName.ProjectName -r CompanyName=VZero -r ProjectName=Demo -o .
vz rn CompanyName.ProjectName.ModuleName -r CompanyName=VZero -r ProjectName=Demo -r ModuleName=IM -o .

```
