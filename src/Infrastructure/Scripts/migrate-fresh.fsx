// To be neater implemented
#r @"../../../bin/Debug/net8.0/grocery-back-office-api.dll"
#load "common.fsx"
#load "migrate-rollback.fsx"
#load "migrate.fsx"

open Common

printf "\n"

MigrateFresh.exec()

