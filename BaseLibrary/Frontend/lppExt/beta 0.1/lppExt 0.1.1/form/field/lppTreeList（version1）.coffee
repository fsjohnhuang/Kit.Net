config  = 
	extend: 'Ext.form.field.ComboBox'
	alias: 'widget.lppTreeList'
	popupHeight: 200
	popupUrl: './d.js'
	defaultValue: ''
	defaultText: ''
	allowMultiSelect: false
	tree: {}
	tmpPDiv: {}
	initComponent: ->
		@treeId = Ext.id()+'-tree'
		@store = null
		@tpl = new Ext.Template('<div id="'+@treeId+'" style="height:' + @popupHeight + 'px;" ></div>')
		@editable = false
		@autoScroll = false
		@minWidth = 275
		@width = 275 if not @width? or @width < 275
		@value = @defaultText
		@hiddenName = @name
		@name = @name + '-tree'
		@on 
			afterrender: =>
				document.getElementsByName(@hiddenName, @bodyEl.dom)[0].value = @defaultValue

		@callParent arguments
	expand: ->
		@callParent arguments

		@tmpDivId = @treeId + (+new Date()) if !@tmpDivId?
		if !@tree[@treeId]?.rendered
			@tree[@treeId] = Ext.create 'lppExt.tree.lppTreeView',
				collapsible: false
				width: 'auto'
				minWidth: 'auto'
				maxWidth: 'auto'
				selModel: if @allowMultiSelect then Ext.create 'Ext.selection.CheckboxModel' else Ext.create 'Ext.selection.RowModel' 
				height: @popupHeight
				toolbar: 
					top: [{
						type: 'localFilter'
						text: ''
					},{
						type: 'clearAll'
						icon: "#{lppExt.util.Resource.IMG}cross.png"
						tooltip: 'Çå¿Õ'
						handler: (item)=>
							@tree[@treeId].getSelectionModel().deselectAll()
							document.getElementsByName(@hiddenName, @bodyEl.dom)[0].value = ''
							@setValue ''
					}]
				autoLoadStore: true
				remoteProxy: @popupUrl
				border: false
				listeners:
					afterrender: (treePanel, eOpt)=>
						@tmpPDiv[@tmpDivId] = treePanel.getEl().dom
						document.getElementById(@treeId).appendChild @tmpPDiv[@tmpDivId]

		
		renderTree = =>
			if @tree[@treeId].rendered
				#document.getElementById(@treeId).innerHTML = ''
				document.getElementById(@treeId).appendChild @tmpPDiv[@tmpDivId]
				#document.getElementById(@treeId).appendChild @tree[@treeId].getEl().dom 
				#document.getElementById(@treeId).innerHTML = @tree[@treeId].getEl().dom.outerHTML
				#document.getElementById(@treeId).innerHTML = 'set';
			else
				div = document.createElement('DIV')
				div.style.display = 'none'
				div.id = @tmpDivId
				document.body.appendChild div
				@tree[@treeId].render div

		setTimeout renderTree, 1

		@on 
			collapse: =>
				selections = @tree[@treeId].getSelectionModel().getSelection()

				if @allowMultiSelect
					texts = ''
					ids = ''
					for selection in selections
						texts += selection.get('text') + '; '
						ids += selection.getId() + ','
					@setValue texts
					ids = ids.substring(0, ids.length - 1) if ids.length >= 1
					document.getElementsByName(@hiddenName, @bodyEl.dom)[0].value = ids
				else
					return if selections.length == 0
					@setValue selections[0].get('text')
					document.getElementsByName(@hiddenName, @bodyEl.dom)[0].value = selections[0].getId()
			destory: =>
				@callParent arguments
				@tree[@treeId].destory()
				delete @tree[@treeId]

Ext.define 'lppExt.form.field.lppTreeList', config
