config  = 
	extend: 'Ext.form.field.ComboBox'
	alias: 'widget.lppTreeList'
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
				@bodyEl.on 'click', =>
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
									tooltip: '清空'
									handler: (item)=>
										@tree.getSelectionModel().deselectAll()
										document.getElementsByName(@hiddenName, @bodyEl.dom)[0].value = ''
										@setValue ''
										@tree.hide()
										Ext.getBody().un 'click', hide_Global
								},{
									type: 'submit'
									icon: "#{lppExt.util.Resource.IMG}check.png"
									tooltip: '确定'
									handler: (item)=>
										@tree.hide()
										Ext.getBody().un 'click', hide_Global
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
					hide_Global = (e, eOpt)=>
						#判断点击处是否为某元素的子元素
						if !!window.find && HTMLElement.prototype.contains?
							HTMLElement.prototype.contains = (B)->
								return this.compareDocumentPosition(B) - 19 > 0 
						if !@tree.getEl().dom.contains(e.target) or e.target == window
							@tree.hide() 
							Ext.getBody().un 'click', hide_Global
					bind_Hide_Global = =>
						if !@tree.isHidden()
							Ext.getBody().on 'click', hide_Global
					setTimeout bind_Hide_Global, 10

					if not @allowMultiSelect
						@tree.on 'itemdblclick', (view, record, item, index, e, eOpts) =>
							if record.isLeaf()
								@setValue record.get('text')
								document.getElementsByName(@hiddenName, @bodyEl.dom)[0].value = record.getId()
								@tree.hide()
								Ext.getBody().un 'click', hide_Global

					@isFirstOpen or= 1
					if @tree.isHidden() || @isFirstOpen++ == 1
						@tree.showAt(@getX()+@labelWidth+5,@getY()+21)
					else
						@tree.hide()
			beforedestroy: ->
				@tree.hide() if @tree?

		@callParent arguments

Ext.define 'lppExt.form.field.lppTreeList', config
