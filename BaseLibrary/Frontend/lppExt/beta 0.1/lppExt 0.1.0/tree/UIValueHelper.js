Ext.define("lppExt.tree.UIValueHelper", {
    statics: {
        getPNodeIDonAdd: function (idProperty) {
            return function _getPNodeIDonAdd(lppTree) {
                var result = { success: false, value: null },
                        selectedModels = lppTree.getSelectionModel().getSelection();
                if (selectedModels.length > 1) return result;

                if (!idProperty) {
                    idProperty = "id";
                }
                var pId = selectedModels.length && selectedModels[0].raw[idProperty] || lppTree.getRootNode().raw[idProperty];
                result.success = true;
                result.value = pId;

                return result;
            };
        },
        getPNodeTextonAdd: function (textProperty) {
            return function _getPNodeTextonAdd(lppTree) {
                var result = { success: false, value: null },
                    selectedModels = lppTree.getSelectionModel().getSelection();
                if (selectedModels.length > 1) return result;

                if (!textProperty) {
                    textProperty = "text";
                }
                var pName = selectedModels.length && selectedModels[0].raw[textProperty] || lppTree.getRootNode().raw[textProperty];
                result.success = true;
                result.value = pName;

                return result;
            };
        },
        getPNodeIDonUpdate: function (idProperty) {
            return function _getPNodeIDonUpdate(lppTree) {
                var result = { success: false, value: null },
                    selectedModels = lppTree.getSelectionModel().getSelection();
                if (selectedModels.length > 1) return result;

                if (!idProperty) {
                    idProperty = "id";
                }
                var pNode = selectedModels.length && selectedModels[0].store.getNodeById(selectedModels[0].raw[idProperty]) || lppTree.getRootNode();
                //var pId = selectedModels.length && selectedModels[0].parentNode.raw[idProperty] || lppTree.getRootNode().raw[idProperty];
                var pId = pNode.raw[idProperty];
                result.success = true;
                result.value = pId;

                return result;
            };
        },
        getPNodeTextonUpdate: function (textProperty, idProperty) {
            return function _getPNodeTextonUpdate(lppTree) {
                var result = { success: false, value: null },
                    selectedModels = lppTree.getSelectionModel().getSelection();
                if (selectedModels.length > 1) return result;

                if (!textProperty) {
                    textProperty = "text";
                }
                var pName = '';
                if (idProperty) {
                    var pNode = selectedModels.length && selectedModels[0].store.getNodeById(selectedModels[0].raw[idProperty]) || lppTree.getRootNode();
                    pName = pNode.raw[textProperty];
                }
                else {
                    pName = selectedModels.length && selectedModels[0].parentNode.raw[textProperty] || lppTree.getRootNode().raw[textProperty];
                }
                result.success = true;
                result.value = pName;

                return result;
            };
        }
    }
});