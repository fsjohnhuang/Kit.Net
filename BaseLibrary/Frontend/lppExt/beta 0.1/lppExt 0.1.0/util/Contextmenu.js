Ext.define("lppExt.util.Contextmenu", {
    requires: ["lppExt.util.Common"],
    statics: {
        resetContextmenuRemotely: function (url, contextmenu) {
            lppExt.util.Common.syncRequest({
                url: url,
                scope: this,
                success: function (response, eOpt) {
                    var obj = Ext.JSON.decode(response.responseText, true);
                    if (obj === null || !obj.success) return;

                    var i;
                    for (i = 0; i < obj.items.length; ++i) {
                        if (obj.items[i].type) {
                            lppExt.util.Operation.update(contextmenu, obj.items[i].type, obj.items[i]);
                        }
                    }
                }
            });

            return contextmenu;
        }
    }
});