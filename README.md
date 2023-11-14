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

## 设置 Git Hook 以统一提交日志格式便于自动生成更新日志

Auto tag and generate changelog.md by `commit-and-tag-version`

```bash
./set-git-hook.sh
npm i -g commit-and-tag-version # https://github.com/absolute-version/commit-and-tag-version#bumpfiles-packagefiles-and-updaters
commit-and-tag-version # --frist-release 
```
