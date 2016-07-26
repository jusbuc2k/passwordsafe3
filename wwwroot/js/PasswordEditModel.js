/// <reference path="../lib/knockout/dist/knockout.debug.js" />
/// <reference path="../lib/knockroute/dist/knockroute.js" />

define(['knockout','knockroute', 'crypto'], function(ko, kr, crypto) {
    'use strict';
    
    function PasswordEditModel(router) {

        this.load = function(routeValues){
            if (routeValues.id) {
                return $.ajax({
                    method: "get",
                    url: "/Password/GetVault/" + routeValues.id,
                    dataType: "json"
                }).then(function(response){
                    this.vaultID = response.vaultID;
                    this.name(response.name);
                }.bind(this));
            } else {
                return true;
            }
        }

        this.save = function(){
            if (this.vaultID === 0) {
                var masterKey = createMasterKey(this.password());
                
                $.ajax({
                    method: "post",
                    url: "/Password/AddVault",
                    data: {
                        name: this.name(),
                        masterKey: masterKey
                    }
                }).then(function(response){
                    router.navigate({ view: "Home" });
                });
            }
        };

        this.vaultID = 0;
        this.name = ko.observable();
        this.password = ko.observable();
    }

    return PasswordEditModel;
});