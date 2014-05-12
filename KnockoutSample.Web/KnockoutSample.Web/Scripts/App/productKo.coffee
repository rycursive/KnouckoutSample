﻿# CoffeeScript
ProductsViewModel = ->
  self = this
  obj = ko.observable()
  self.products = ko.observableArray()
  self.loading = ko.observable(true)
  self.addmode = ko.observable(false)
  self.editMode = ko.observable(false)
  baseUri = "api/Products"
  self.toggleAddMode = ->
    self.addmode not self.addmode()
    return

  self.beginEditMode = (object) ->
    obj object
    return

  self.endEditMode = (object) ->
    obj null
    return

  self.deleteObject = (object) ->
    $.ajax(
      type: "DELETE"
      url: baseUri + "/" + object.Id
    ).done((data, textStatus, xhr) ->
      self.products.remove object
      toastr.success textStatus
      return
    ).fail (xhr, textStatus, error) ->
      toastr.error error
      return

    return

  self.updateObject = (object) ->
    $.ajax(
      type: "PUT"
      url: baseUri + "/" + object.Id
      data: object
    ).done((data, textStatus, xhr) ->
      toastr.success textStatus
      return
    ).fail (xhr, textStatus, error) ->
      toastr.error error
      return

    return

  self.isEditMode = (object) ->
    return ko.computed(->
      obj() is object
    ).extend(notify: "always")()
    return

  self.create = (formElement) ->
    $(formElement).validate()
    if $(formElement).valid()
      $.post(baseUri, $(formElement).serialize(), null, "json").done((o) ->
        self.products.push o
        self.toggleAddMode()
        toastr.success "Adding success"
        return
      ).fail (xhr, textStatus, error) ->
        toastr.error error
        return

    return

  $.getJSON(baseUri, self.products)
    .done -> 
        self.loading false
        return
    .fail -> 
        toastr.error('Loading error')
        return
  return
$(document).ready ->
  ko.applyBindings new ProductsViewModel()
  return
