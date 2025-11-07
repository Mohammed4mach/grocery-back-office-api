// To be neater implemented
#r @"../../../bin/Debug/net8.0/grocery-back-office-api.dll"
#load "common.fsx"

open Common

printf "\n"

Migrate.exec()

