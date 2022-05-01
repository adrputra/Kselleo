const getBoardDetailById = (id, userId) => {
   $.ajax({
      type: 'GET',
      url: `https://localhost:5001/api/boards/detail/${id}`,
      dataType: 'json',
      success: function (response) {
         renderBoardDetail(response, userId)
         renderList(response.data.lists, userId)
      },
      error: function (e) {
         location.href = '/boards'
      },
   })
}

const renderBoardDetail = (response, userId) => {
   console.log(response)
   $('#title').html(response.data.name)
   $('#description').html(response.data.description)
   $('#createdAt').html(moment(response.data.createdAt).format('LLL'))

   let html = `<img src="https://ui-avatars.com/api/?name=${response.data.user.fullName}&background=random" alt="${response.data.user.fullName}" width="40px"
   class="rounded-circle" data-toggle="tooltip" data-placement="right" title="${response.data.user.fullName} - PM"
   style="margin-right: -20px;">`

   // append html to tag #members
   response.data.memberBoards
      .filter((member) => member.role != 'PM')
      .map((member) => {
         html += `<img src="https://ui-avatars.com/api/?name=${member.user.fullName}&background=random" alt="${member.role}" width="40px"
      class="rounded-circle" data-toggle="tooltip" data-placement="right" title="${member.user.fullName} - ${member.role}"
      style="margin-right: -20px;">`
      })

   $('#members').append(html)

   if (response.data.user.id == userId) {
      $('#display-btn-invite').append(
         `
         <button class="btn btn-invite mr-4" style="background-color: #FAF5E4; color: #8B8B8B"
         data-bs-toggle="modal"
         data-bs-target="#inviteModal">+ Invite Member</button>
         `
      )
   }
}

const createList = (userId) => {
   event.preventDefault()
   event.stopPropagation()

   const test = $('#name-list').val()
   const req = {
      BoardId: $('#boardId').val(),
      Name: $('#name-list').val(),
      CreatedBy: userId,
      Status: 'Todo',
   }

   $.ajax({
      type: 'POST',
      url: `https://localhost:5001/api/lists`,
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      dataType: 'json',
      data: JSON.stringify(req),
      success: function (response) {
         swal('Success!', 'Your board has been created!', 'success')
         location.reload()
         $('#createListModal').modal('hide')
      },
      error: function (e) {
         swal('Error!', `${JSON.parse(e.responseText).message}`, 'error')
      },
   })
}

const renderList = (lists, userId) => {
   const todo = lists.filter((list) => list.status == 'Todo')
   const inProgress = lists.filter((list) => list.status == 'In Progress')
   const review = lists.filter((list) => list.status == 'Review')
   const done = lists.filter((list) => list.status == 'Done')

   renderListByStatus(todo, 'todos', userId)
   renderListByStatus(inProgress, 'in-progress', userId)
   renderListByStatus(review, 'review', userId)
   renderListByStatus(done, 'done', userId)
}

const renderListByStatus = (items, status, userId) => {
   let html = ``
   items.forEach((item, i) => {
      console.log('list', item)

      html += `
      <div class="list-item rounded shadow mt-4" style="padding: 18px; background-color: #FAF5E4;">
            <div class="list-item-header d-flex justify-content-between align-items-center">
               <p style="font-size: 18px; color: #272727; font-weight: 500; margin-block: auto;">${
                  item.name
               }</p>
               <div>
                  ${
                     item.user.id == userId
                        ? `<button class="btn btn-outline-warning btn-sm" onclick="updateListModal(${item.id}, '${item.name}', ${item.user.id},'${item.status}', '${item.boardId}')">
                     <i class="fa fa-pencil" aria-hidden="true"></i>
                  </button>
                  <button class="btn btn-outline-danger btn-sm" onclick="deleteList(${item.id})">
                     <i class="fa fa-trash" aria-hidden="true"></i>
                  </button>`
                        : ''
                  }
               </div>
            </div>

            <div class="cards mt-4">
               ${renderCard(item.cards, userId)}
            </div>

            ${
               item.user.id == userId
                  ? `<a class="m-auto w-100 pt-4"
               style="text-decoration: underline; text-align: center; cursor: pointer; color: #736D6D" type="button" onclick="createCardModal(${item.id}, '${item.name}')">+
               Add
               a card</a>`
                  : ''
            }
      </div>
      `
   })

   $(`#${status}`).append(html)
}

const renderCard = (cards, userId) => {
   let html = ``
   cards.forEach((card, i) => {
      console.log('card', card)
      html += `
         <button id="card-detail" class="btn shadow rounded bg-white mb-3 d-flex justify-content-between flex-column" style="padding: 10px; width: 100%;" onclick="openDetailCard(${
            card.id
         })">
            <p style="color: #717171; text-align: left; font-size: 16px">${
               card.name
            }</p>

            <div class="d-flex justify-content-between w-100 " style="align-items: center;">
               <p style="font-size: 14px; color: #717171; margin: 0px;"> Due ${moment(
                  card.due
               ).format('l')}
               </p>


                  <div>
                  <img src="https://ui-avatars.com/api/?name=test&background=random" alt="test" width="25px"
                     class="rounded-circle" data-toggle="tooltip" data-placement="right" title="test - PM""
                     </div>
                  
            </div>
         </button>`
   })

   return html
}

const openDetailCard = (cardId) => {
   $.ajax({
      type: 'GET',
      url: `https://localhost:5001/api/cards/detail/${cardId}`,
      data: 'data',
      dataType: 'json',
      success: function (response) {
         console.log(response)

         $('#name-card-detail').html(response.data.name)
         $('#name-list-detail').html('in list ' + response.data.list.name)
         $('#description-card-detail').html(response.data.description)
      },
   })

   $('#detailCardModal').modal('show')
}

const createCardModal = (listId) => {
   $('#list-id-card').val(listId)
   $('#createCardModal').modal('show')
}

const createCard = (userId) => {
   event.preventDefault()
   event.stopPropagation()

   /**
    * ListId
    * Name
    * CreatedBy
    * Due
    */
   const req = {
      ListId: parseInt($('#list-id-card').val()),
      Name: $('#name-card').val(),
      Description: $('#description-card').val().replace(/\n/g, ' '),
      CreatedBy: userId,
      Due: $('#due-card').val(),
   }

   $.ajax({
      type: 'POST',
      url: `https://localhost:5001/api/cards`,
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      dataType: 'json',
      data: JSON.stringify(req),
      success: function (response) {
         swal('Success!', 'Your card has been created!', 'success')
         location.reload()
         $('#createCardModal').modal('hide')
      },
      error: function (e) {
         swal('Error!', `${JSON.parse(e.responseText).message}`, 'error')
      },
   })
}

const updateListModal = (id, name, createdBy, status, boardId) => {
   $('#id-list-update').val(id)
   $('#createdby-list-update').val(createdBy)
   $('#boardid-list-update').val(boardId)
   $('#name-list-update').val(name)
   $('#status-list-update').val(status)
   $('#updateListModal').modal('show')
}

const updateList = () => {
   event.preventDefault()

   const req = {
      Id: parseInt($('#id-list-update').val()),
      Name: $('#name-list-update').val(),
      Status: $('#status-list-update').val(),
      CreatedBy: parseInt($('#createdby-list-update').val()),
      BoardId: $('#boardid-list-update').val(),
   }

   $.ajax({
      type: 'PUT',
      url: `https://localhost:5001/api/lists`,
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      dataType: 'json',
      data: JSON.stringify(req),
      success: function (response) {
         swal('Success!', 'Your list has been updated!', 'success')
         location.reload()
         $('#updateListModal').modal('hide')
      },
      error: function (e) {
         swal('Error!', `${JSON.parse(e.responseText).message}`, 'error')
      },
   })
}

const deleteList = (id) => {
   event.stopPropagation()
   event.preventDefault()

   swal({
      title: 'Are you sure?',
      text: 'Once deleted, you will not be able to recover this list!',
      icon: 'warning',
      buttons: true,
      dangerMode: true,
   }).then((willDelete) => {
      if (willDelete) {
         $.ajax({
            type: 'DELETE',
            url: `https://localhost:5001/api/lists/${id}`,
            success: function (response) {
               swal('Poof! Your list has been deleted!', {
                  icon: 'success',
               })
               location.reload()
            },
            error: function (response) {
               swal('upps! delete failed', {
                  icon: 'error',
               })
            },
         })
      } else {
         swal('Your list is safe!')
      }
   })
}

const inviteMember = () => {
   event.preventDefault()

   const req = {
      Email: $('#email').val(),
      Role: $('#role').val(),
      BoardId: $('#boardId').val(),
   }

   $.ajax({
      type: 'POST',
      url: 'https://localhost:5001/api/verifyinvites/verify',
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      data: JSON.stringify(req),
      success: function (response) {
         $('#inviteModal').modal('hide')
         $('#form-invite')[0].reset()
         swal('Horee! Your invitation has been sent, waiting to be accepted!', {
            icon: 'success',
         })
      },
      error: function (e) {
         swal(`upps! ${JSON.parse(e.responseText).message}`, {
            icon: 'error',
         })
      },
   })
}

const getListByBoardId = (id) => {
   // console.log('ASdasd', id)
}
