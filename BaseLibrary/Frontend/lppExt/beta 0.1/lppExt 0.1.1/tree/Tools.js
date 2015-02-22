(function() {
  var config;

  config = {
    statics: {
      changeIndex: function(node, data, overModel, dropPosition, dropHandlers, url) {
        var newIndex, newPID, sourceIndex, sourcePID;

        sourcePID = data.records[0].parentNode.raw.ID || data.records[0].parentNode.raw.id;
        sourceIndex = data.records[0].parentNode.indexOf(data.records[0]);
        if (dropPosition === 'before') {
          if (overModel.parentNode.raw.ID === sourcePID || overModel.parentNode.raw.id === sourcePID) {
            newPID = overModel.parentNode.raw.ID || overModel.parentNode.raw.id;
          }
          newIndex = overModel.parentNode.indexOf(overModel);
          if (newPID === sourcePID && newIndex > sourceIndex) {
            newIndex -= 1;
          }
          if (newIndex === -1) {
            newIndex = 0;
          }
        } else if (dropPosition === 'after') {
          if (overModel.parentNode.raw.ID === sourcePID || overModel.parentNode.raw.id === sourcePID) {
            newPID = overModel.parentNode.raw.ID || overModel.parentNode.raw.id;
          }
          newIndex = overModel.parentNode.indexOf(overModel);
          newIndex = newIndex > sourceIndex && newPID === sourcePID ? newIndex : newIndex + 1;
        } else if (dropPosition === 'append') {
          newPID = overModel.raw.ID || overModel.raw.id;
          newIndex = overModel.childNodes.length;
        }
        return Ext.Ajax.request({
          method: 'POST',
          params: {
            id: data.records[0].raw.id || data.records[0].raw.ID,
            sourcePID: sourcePID,
            sourceIndex: sourceIndex,
            newPID: newPID,
            newIndex: newIndex
          },
          url: url
        });
      }
    }
  };

  Ext.define('lppExt.tree.Tools', config);

}).call(this);
