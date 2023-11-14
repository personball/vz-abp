# readme

## Base On

``` bash
mkdir CompanyName.ProjectName.ModuleName
cd CompanyName.ProjectName.ModuleName
abp new CompanyName.ProjectName.ModuleName -t module --no-ui
```

- replace sqlserver with postgresql

## Next TODO

- 修改 StringEncryption.DefaultPassPhrase
- 需要 redis 实例，默认密码 123qwe
- 需要 postgresql 实例，默认密码 myPassword，用 postgres 账号


## setup git hook to enforce commit-msg format

```bash
./set-git-hook.sh
conventional-changelog -p angular -i CHANGELOG.md -s # TODO: https://github.com/absolute-version/commit-and-tag-version#bumpfiles-packagefiles-and-updaters
```