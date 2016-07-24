/// <reference path="../lib/knockout/dist/knockout.debug.js" />
/// <reference path="../lib/knockroute/dist/knockroute.js" />
/// <reference path="knockroute.js" />

define(['knockout','knockroute'], function(ko, kr) {
    'use strict';
    
    function HomeModel() {

        this.load = function(){
            return true;
        }

        this.title = ko.observable("Hello World!");
        
    }

    return HomeModel;
});