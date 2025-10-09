# readme

## Base On

``` bash
mkdir CompanyName.ProjectName.ModuleName
cd CompanyName.ProjectName.ModuleName
abp new CompanyName.ProjectName.ModuleName -t module -dbms PostgreSQL --no-ui
```

- replace sqlserver with postgresql

## Next TODO

- 修改 StringEncryption.DefaultPassPhrase
- 需要 redis 实例，默认密码 123qwe
- 需要 postgresql 实例，默认密码 123qwe，用 postgres 账号

## setup git hook to enforce commit-msg format

Auto tag and generate changelog.md by `commit-and-tag-version`
[conventional commits](https://www.conventionalcommits.org/en/v1.0.0/#summary)

```bash
./set-git-hook.sh
npm i -g commit-and-tag-version # https://github.com/absolute-version/commit-and-tag-version#bumpfiles-packagefiles-and-updaters
commit-and-tag-version # --frist-release 
```

config `.versionrc` follow: [conventional-changelog-config-spec](https://github.com/conventional-changelog/conventional-changelog-config-spec/blob/master/versions/2.2.0/schema.json)
