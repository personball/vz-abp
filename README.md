vz project templates

# backend
abp.io

## microservice:

BaseOn:

``` bash
dotnet tool install --global Volo.Abp.Cli --version 9.3.5

abp new CompanyName.ProjectName -dbms PostgreSQL --tiered --theme basic -csf -v 9.3.5

mkdir CompanyName.ProjectName.ModuleName
cd CompanyName.ProjectName.ModuleName
abp new CompanyName.ProjectName.ModuleName -t module -dbms PostgreSQL --no-ui -v 9.3.5
```

*Note:* `DateTime` 已配置为 `DateTimeKind.Utc`


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
[conventional commits](https://www.conventionalcommits.org/en/v1.0.0/#summary)

```bash
./set-git-hook.sh # For windows can be executed in git-bash
npm i -g commit-and-tag-version # https://github.com/absolute-version/commit-and-tag-version#bumpfiles-packagefiles-and-updaters
commit-and-tag-version # --frist-release 
```

config `.versionrc` follow: [conventional-changelog-config-spec](https://github.com/conventional-changelog/conventional-changelog-config-spec/blob/master/versions/2.2.0/schema.json)
