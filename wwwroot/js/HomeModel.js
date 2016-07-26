/// <reference path="../lib/knockout/dist/knockout.debug.js" />
/// <reference path="../lib/knockroute/dist/knockroute.js" />

define(['knockout','knockroute'], function(ko, kr) {
    'use strict';
    
    function HomeModel() {

        this.load = function(){
            return $.ajax({
                method: "get",
                url: "/Password/ListVaults",
                dataType: "json"
            }).then(function(response){
                this.vaults(response);
            }.bind(this), function(err){
                console.error(err);                
            });
        }

        this.vaults = ko.observableArray();        
    }

    return HomeModel;
});