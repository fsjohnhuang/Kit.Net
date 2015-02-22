config  = 
	extend: 'Ext.form.field.ComboBox'
	alias: 'widget.lppTreeList2'
	popupHeight: 200
	popupUrl: './d.js'
	defaultValue: ''
	defaultText: ''
	allowMultiSelect: false
	tree: null
	initComponent: ->
		@treeId = Ext.id()+'-tree'
		@store = null
		@editable = false
		@autoScroll = false
		@minWidth = 280
		@width = 280 if not @width? or @width < 280
		@value = @defaultText
		@hiddenName = @name
		@name = @name + '-tree'
		@on 
			afterrender: =>
				document.getElementsByName(@hiddenName, @bodyEl.dom)[0].value = @defaultValue

		@callParent arguments
	expand: ->
		@callParent arguments

		if !@tree?.rendered
			@tree = Ext.create 'lppExt.tree.lppTreeView',
				collapsible: false
				width: @inputEl.dom.offsetWidth + @triggerEl.elements[0].dom.offsetWidth
				minWidth: 'auto'
				maxWidth: 'auto'
				floating: true
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
							@tree.getSelectionModel().deselectAll()
							document.getElementsByName(@hiddenName, @bodyEl.dom)[0].value = ''
							@setValue ''
							@tree.hide()
					},{
						type: 'submit'
						icon: "#{lppExt.util.Resource.IMG}check.png"
						tooltip: 'È·¶¨'
						handler: (item)=>
							@tree.hide()
							selections = @tree.getSelectionModel().getSelection()
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
					}]
				autoLoadStore: true
				remoteProxy: @popupUrl
		@tree.showAt(@getX()+@labelWidth+5,@getY()+22);

		on_click = =>
			@tree.hide()
			@bodyEl.un 'click', on_click
		set_on_click = =>
			@bodyEl.on 'click', on_click
		setTimeout set_on_click, 20

		@on 
			###collapse: =>
				selections = @tree.getSelectionModel().getSelection()
				@tree.hide()

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
					###
			destory: =>
				@callParent arguments
				@tree.destory()
				delete @tree[@treeId]

Ext.define 'lppExt.form.field.lppTreeList2', config
