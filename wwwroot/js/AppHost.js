define(["knockout", "knockroute", "HomeModel", "VaultEditModel", "VaultModel"], function(ko, kr, HomeModel, VaultEditModel, VaultModel){

    return function AppHost() {

        this.router = new kr.ViewRouter({
            routes: [
                {
                    template: "{view}/{id?}",
                    defaults: {
                        view: "Home"
                    }
                }
            ],
            views: [
                { name: "Home", model: HomeModel, templateSrc: "templates/Home.html", templateID: "t1" },
                { name: "AddVault", model: VaultEditModel, templateSrc: "templates/VaultEdit.html", templateID: "t2" },
                { name: "Vault", model: VaultModel, templateSrc: "templates/Vault.html", templateID: "t3"}
            ],
            templateProvider: new kr.AjaxTemplateProvider({
                useTags: false
            }),
            defaultContent: "Loading, please wait..."
        });
        
    }

});