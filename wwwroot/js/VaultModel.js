define(['knockout', 'knockroute', 'http'], function(ko, kr, http) {
    'use strict';

    function PasswordViewModel(router){
        this.name = ko.observable();
        this.description = ko.observable();
        this.data = ko.observable();

        this.load = function(routeValues){
            this.name(routeValues.name);
            this.description(routeValues.description);
            this.data(routeValues.data);
        };
    }

    function PasswordListModel(router){
        this.passwords = ko.observableArray();

        this.passwordClicked = function(password){
            http.post("/Password/Decrypt/" + password.passwordID).then(function(response){
                router.setView({
                    name: 'PasswordView',
                    model: PasswordViewModel,
                    templateID: 'password_view',
                    templateSrc: '/templates/PasswordView.html' 
                }, response);
            });
        }.bind(this);

        this.load = function(routeValues){
            this.passwords(routeValues);
        };
    }

    return function VaultModel(router) {
        this.vaultID = null;
        this.title = ko.observable();
        this.passwords = ko.observableArray();

        this.unlockPassword = ko.observable();
        this.isUnlocked = ko.observable(false);

        this.unlockClicked = function(){
            http.post('/Password/Unlock/' + this.vaultID, {
                password: this.unlockPassword()
            }).then(function(response) {
                this.passwords(response);
                this.isUnlocked(true);

                router.setView({
                    name: 'PasswordList',
                    model: PasswordListModel,
                    templateID: 'password_list',
                    templateSrc: '/templates/PasswordList.html'
                }, response);
            }.bind(this), function(err){
                alert('invalid password');
            });
        }.bind(this);

        this.load = function(routeValues) {
            this.vaultID = routeValues.id;
            return true;
        };
    }

});