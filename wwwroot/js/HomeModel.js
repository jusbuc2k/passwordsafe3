define(['knockout','knockroute', 'http'], function(ko, kr, http) {
    'use strict';
    
    function HomeModel() {

        this.load = function(){
            return http.get("/Password/ListVaults").then(function(response){
                this.vaults(response);
            }.bind(this));
	    }

        this.vaults = ko.observableArray();        
    }

    return HomeModel;
});