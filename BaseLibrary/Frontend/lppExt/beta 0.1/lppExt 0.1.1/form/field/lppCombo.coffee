config = 
	extend: 'Ext.form.field.ComboBox'
	alias: 'widget.lppCombo'
	queryMode: 'local'
	forceSelection: true
	editable: true
	readOnly: false
	triggerAction: 'all'
	emptyText: 'ÇëÑ¡Ôñ...'
	url: ''
	listeners:
		beforequery: (e, eOpts)->
			combo = e.combo
			if (!e.forceAll)
				value = e.query
				combo.store.filterBy (record, id) ->
					text = record.get combo.displayField
					text.indexOf(value) >= 0
				combo.expand()
				false
	initComponent: ->
		@store = Ext.create 'Ext.data.Store',
			autoLoad: true
			fields: @fields
			proxy:
				type: 'ajax'
				method: 'GET'
				url: @url
				reader:
					root: 'items'
			listeners:
				load: (store, records, successful, eOpt)=>
					newModel = {}
					for field in @fields
						newModel.field = ''
					store.insert(0, newModel)
		@callParent arguments

Ext.define 'lppExt.form.field.lppCombo', config