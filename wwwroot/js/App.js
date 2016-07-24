define(["knockout", "knockroute", "HomeModel"], function(ko, kr, HomeModel){

    return function() {
        this.router = new kr.ViewRouter({
            views: [
                { name: "home", model: HomeModel, templateSrc: "templates/home.html", templateID: "router_tmpl_home" }
            ],
            templateProvider: new kr.AjaxTemplateProvider({
                useTags: false
            })
        });
    }

});