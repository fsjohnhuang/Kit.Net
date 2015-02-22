config = 
	statics:
		changeIndex: (node, data, overModel, dropPosition, dropHandlers, url)->
			sourcePID = data.records[0].parentNode.raw.ID or data.records[0].parentNode.raw.id
			sourceIndex = data.records[0].parentNode.indexOf(data.records[0])
			if dropPosition == 'before'
				newPID = overModel.parentNode.raw.ID or overModel.parentNode.raw.id if overModel.parentNode.raw.ID is sourcePID || overModel.parentNode.raw.id is sourcePID
				newIndex = overModel.parentNode.indexOf(overModel)
				newIndex -= 1 if newPID == sourcePID && newIndex > sourceIndex
				newIndex = 0 if newIndex == -1
			else if dropPosition == 'after'
				newPID = overModel.parentNode.raw.ID or overModel.parentNode.raw.id if overModel.parentNode.raw.ID is sourcePID || overModel.parentNode.raw.id is sourcePID
				newIndex = overModel.parentNode.indexOf(overModel)
				newIndex = if newIndex > sourceIndex and newPID == sourcePID then newIndex else newIndex + 1
			else if dropPosition == 'append'
				newPID = overModel.raw.ID or overModel.raw.id
				newIndex = overModel.childNodes.length
			Ext.Ajax.request
				method: 'POST'
				params:
					id: data.records[0].raw.id || data.records[0].raw.ID
					sourcePID: sourcePID
					sourceIndex: sourceIndex
					newPID: newPID
					newIndex: newIndex
				url: url


Ext.define 'lppExt.tree.Tools', config