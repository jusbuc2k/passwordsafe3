import * as ko from "knockout";
import {kr} from "knockroute";
import {HomeModel} from "HomeModel";

export class AppHost{
    
    router:any;

    foo:any;

    constructor(){
        this.foo = ko.observable();
        this.router = new (<any>kr).ViewRouter({
            views: [
                { name: "home", model: HomeModel, templateSrc: "templates/home.html", templateID: "router_tmpl_home" }
            ],
            templateProvider: new (<any>kr).AjaxTemplateProvider({
                useTags: false
            }),
            defaultContent: "Loading, please wait..."
        });
    }
}
