@echo off
echo ******************************************************
echo ********************* CACHE CLEAN ********************
echo ******************************************************
call npm cache clean --force
echo ******************************************************
echo ******************* Actualizar NPM *******************
echo ******************************************************
rem call npm install -g npm@latest
call npm install --global npm@6
echo ******************************************************
echo ***************** Actualizar Angular *****************
echo ******************************************************
call npm install -g @angular/cli@latest
echo ******************************************************
echo ***************** Instalar Components ****************
echo ******************************************************
call npx browserslist@latest --update-db
call npm install
echo ******************************************************
echo **************** Actualizar Components ***************
echo ******************************************************
call npm update
echo ******************************************************
echo ********** Parchar y coregir vulnerbilidades *********
echo ******************************************************
call npm audi fix
