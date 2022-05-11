const getBoardDetailById = (id, userId) => {
   $.ajax({
      type: 'GET',
      url: `https://localhost:5001/api/boards/detail/${id}`,
      dataType: 'json',
      success: function (response) {
         renderBoardDetail(response, userId)
         renderList(response.data.lists, userId)

         // render members to modal create task -> select members.
         response.data.memberBoards
            .filter((member) => member.role != 'PM')
            .map((member) => {
               $('#members-task').append(
                  `<option value="${member.user.id}">${member.user.fullName}</option>`
               )
            })
      },
      error: function (e) {
         location.href = '/boards'
      },
   })
}

const renderBoardDetail = (response, userId) => {
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

   // display btn invite and btn create list
   if (response.data.user.id == userId) {
      $('#display-btn-invite').append(
         `
         <button class="btn btn-invite mr-4" style="background-color: #FAF5E4; color: #8B8B8B"
         data-bs-toggle="modal"
         data-bs-target="#inviteModal">+ Invite Member</button>
         `
      )

      $('#display-btn-create-list').html(
         `
         <div class="create-list" style="position: fixed; bottom: 30px; right: 50px; ">
            <button class="btn text-white" style="background-color: #F8B401;" data-bs-toggle="modal"
               data-bs-target="#createListModal">+ Create List</button>
         </div>
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
   const todo = lists.filter((list) => list.status == 'Todo').reverse()
   const inProgress = lists
      .filter((list) => list.status == 'In Progress')
      .reverse()
   const review = lists.filter((list) => list.status == 'Review').reverse()
   const done = lists.filter((list) => list.status == 'Done').reverse()

   renderListByStatus(todo, 'todos', userId)
   renderListByStatus(inProgress, 'in-progress', userId)
   renderListByStatus(review, 'review', userId)
   renderListByStatus(done, 'done', userId)
}

const renderListByStatus = (items, status, userId) => {
   let html = ``
   items.forEach((item, i) => {
      html += `
      <div class="list-item rounded shadow mt-4" style="padding: 18px; background-color: #FAF5E4;">
            <div class="list-item-header d-flex justify-content-between align-items-start">
               <div>
               <p style="font-size: 18px; color: #272727; font-weight: 500; margin-block: auto;">${
                  item.name
               }</p>
               <p style="font-size: 12px; color: #535454; font-weight: 300; margin-block: auto;">Created at ${moment(
                  item.createdAt
               ).format('ll')}
               </p>
               </div>
               
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
      html += `
         <button id="card-detail" class="btn shadow rounded bg-white mb-3 d-flex justify-content-between flex-column" style="padding: 10px; width: 100%;" onclick="openDetailCard(${
            card.id
         }, ${userId})">
            <p style="color: #717171; text-align: left; font-size: 16px">${
               card.name
            }</p>

            <div class="d-flex justify-content-between w-100 " style="align-items: center;">
               <p style="font-size: 11px; color: #717171; margin: 0px;"> Created at ${moment(
                  card.createdAt
               ).format('ll')}
               </p>
               <div>
                  ${renderMemberCards(card.checkListItems)}
               </div>
            </div>
         </button>`
   })

   return html
}

const renderMemberCards = (checkListItems) => {
   const ids = []
   const members = checkListItems.map((item) => {
      return item.checkListItemAssigns.map((assign) => {
         return assign.user
      })
   })

   const uniqueMembers = []

   members.forEach((items) => {
      items.forEach((item) => {
         if (!ids.includes(item.id)) {
            ids.push(item.id)
            uniqueMembers.push(item)
         }
      })
   })

   let html = ``
   uniqueMembers.forEach((member) => {
      html += `
      <img src="https://ui-avatars.com/api/?name=${member.fullName}&background=random" alt="${member.fullName}" width="25px"
                        class="rounded-circle" data-toggle="tooltip" data-placement="right" title="${member.fullName}">
                     </img>
            `
   })
   return html
}

const openDetailCard = (cardId, userId) => {
   $.ajax({
      type: 'GET',
      url: `https://localhost:5001/api/cards/detail/${cardId}`,
      data: 'data',
      dataType: 'json',
      success: function (response) {
         const userId = parseInt($('#userId').val())
         console.log('resss', response)

         $('#card-id').val(response.data.id)
         $('#name-card-detail').html(response.data.name)
         $('#name-list-detail').html('in list ' + response.data.list.name)
         $('#description-card-detail').html(response.data.description)

         // card update
         $('#card-id-card-update').val(response.data.id)
         $('#list-id-card-update').val(response.data.listId)
         $('#name-card-update').val(response.data.name)
         $('#description-card-update').val(response.data.description)

         $('#due-card-update').val(convertDateToInput(response.data.due))

         // render edit and delete card
         if (userId == response.data.createdBy) {
            $('.display-actions-card').html(
               `
               <button class="btn btn-outline-danger mr-1" onclick="deleteCard()">
               <i class="fa fa-trash" aria-hidden="true"></i>
            </button>
            <button class="btn btn-outline-warning" data-bs-toggle="modal" data-bs-target="#updateCardModal">
               <i class="fa fa-pencil" aria-hidden="true"></i>
            </button>
               `
            )
         }

         // render progressbar
         const totalTask = response.data.checkListItems.length
         const isChecked = response.data.checkListItems.filter(
            (x) => x.isChecked == true
         )

         let progress = 0

         if (totalTask > 0 && isChecked.length > 0) {
            progress = (isChecked.length / totalTask) * 100
         }

         $('#progress-tasks').html(
            `
            <div class="progress-bar" role="progressbar" style="width: ${progress}%;" aria-valuenow="${progress}" aria-valuemin="0"
            aria-valuemax="100">${progress}%</div>
            `
         )

         // render tasks
         let checkListHTML = ''
         response.data.checkListItems.map((x) => {
            let assignTasks = ''
            let assignIds = x.checkListItemAssigns.map((y) => y.userId)

            x.checkListItemAssigns.map((item) => {
               assignTasks += `
               <img src="https://ui-avatars.com/api/?name=${item.user.fullName}&background=random" alt="${item.user.fullName}" width="25px"
               class="rounded-circle" data-toggle="tooltip" data-placement="right" title="${item.user.fullName} - ${item.user.memberBoards[0].role}"></img>
            `
            })

            checkListHTML += `
            <div class="task d-flex justify-content-between align-items-center">
                     <div class="form-check" style="flex: 0.5;">
                        <input class="form-check-input" type="checkbox" value="${
                           x.isChecked
                        }" id="${x.id}" ${
               x.isChecked == true && 'checked'
            } onclick="checkingTask(${x.id}, ${x.isChecked})" ${
               assignIds.includes(userId) || userId == response.data.createdBy
                  ? ''
                  : 'disabled'
            }>
                        <label class="form-check-label text-secondary" for="${
                           x.id
                        }">
                           ${x.name}
                        </label>
                     </div>

                     <div class="detail-task d-flex justify-content-end align-items-center" style="gap: 12px; flex: 0.5;">
                        <div class="due-task text-secondary"> ${moment(
                           x.startDate
                        ).format('LL')} - ${moment(x.due).format('LL')} </div>
                        <div class="assisn-task d-flex justify-content-between align-items-center" style="gap: 8px;">
                          ${assignTasks}
                        </div>
                     </div>

                    
                     ${
                        userId == response.data.createdBy
                           ? `<div class="detail-task-action ml-4">
                        <div class='buttons d-flex' style='gap: 5px;'>
                           <div>
                              <button class='btn btn-outline-warning btn-sm mr-1' onclick="openModalUpdateTask(${x.id})">
                                   <i
                                      class='fa fa-pencil'
                                      aria-hidden='true'
                                   ></i>
                              </button>
                           </div>
                           <div>
                               <button class='btn btn-outline-danger btn-sm' onclick="deleteTask(${x.id})">
                               <i
                                  class='fa fa-trash'
                                  aria-hidden='true'
                               ></i>
                            </button>
                           </div>
                        </div>
                     </div>`
                           : ''
                     }
                    
                  </div>
            `
         })

         $('.tasks').html(checkListHTML)

         // render btn add task
         if (userId == response.data.createdBy) {
            $('#display-btn-add-task').html(
               `
               <button class="btn-add-task btn text-center m-auto" data-bs-toggle="modal"
               data-bs-target="#createTaskModal">
               + add a task
            </button>
            `
            )
         }

         // render comments
         let commentHTML = ''
         response.data.comments.map((x) => {
            commentHTML += `
            <div class="row">
               <div class="col-md-1">
                  <img src="https://ui-avatars.com/api/?name=${
                     x.user.fullName
                  }&background=random" alt="${x.user.fullName}" width="40px"
                     class="rounded-circle" data-toggle="tooltip" data-placement="right" title="${
                        x.user.fullName
                     }">
                  </img>
               </div>
               <div class="col-md-10">
                  <p style="font-size: 14px;" class="text-secondary">
                     ${x.text}
                  </p>
                 <p class="text-secondary" style="font-size: 12px;">
                     ${moment(x.createdAt).format('lll')}
                  </p>
               </div>
               <div class="col-md-1">
                  ${
                     x.userId == userId
                        ? `
                     <button class="btn btn-outline-danger mr-1 btn-sm" onclick="deleteComment(${x.id})">
                     <i class="fa fa-trash" aria-hidden="true"></i>
                  </button>`
                        : ''
                  }
               </div>
            </div>
            `
         })

         $('.comments').html(commentHTML)
      },
   })

   $('#detailCardModal').modal('show')
}

const checkingTask = (taskId, isChecked) => {
   event.preventDefault()

   // ajax method put /cards/task
   $.ajax({
      type: 'PUT',
      url: `https://localhost:5001/api/cards/task/checking`,
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      dataType: 'json',
      data: JSON.stringify({ Id: parseInt(taskId), IsChecked: isChecked }),
      success: function (response) {
         swal('Success!', 'Your task has been checking!', 'success')
         setTimeout(() => {
            location.reload()
         }, 1000)
      },
      error: function (e) {
         swal('Error!', `${JSON.parse(e.responseText).message}`, 'error')
      },
   })
}

const openModalUpdateTask = (taskId) => {
   const members = []

   $('#members-task option').each(function () {
      members.push({ id: parseInt($(this).val()), name: $(this).text() })
   })

   // get checklistitems by taskId
   $.ajax({
      type: 'GET',
      url: `https://localhost:5001/api/checklistitems/detail/${taskId}`,
      data: 'data',
      dataType: 'json',
      success: function (response) {
         $('#id-task-update').val(taskId)
         // show modal update task
         $('#name-task-update').val(response.data.name)

         // render members to modal create task -> select members.
         let optionHTML = ''

         const assignIds = response.data.checkListItemAssigns.map(
            (x) => x.user.id
         )

         members.map((member) => {
            optionHTML += `<option value="${member.id}" ${
               assignIds.includes(member.id) && 'selected'
            }>${member.name}</option>`
         })
         console.log(response.data)

         $('#members-task-update').html(optionHTML)

         $('#start-task-update').val(
            convertDateToInput(response.data.startDate)
         )
         $('#due-task-update').val(convertDateToInput(response.data.due))

         $('#updateTaskModal').modal('show')
      },
   })
}

const updateTask = (userId) => {
   event.preventDefault()

   const req = {
      Id: parseInt($('#id-task-update').val()),
      Name: $('#name-task-update').val(),
      Members: $('#members-task-update').val(),
      StartDate: $('#start-task-update').val(),
      Due: $('#due-task-update').val(),
      CardId: parseInt($('#card-id').val()),
   }

   if (req.Members.length === 0) {
      return swal('Oops!', 'You need to add at least one member!', 'error')
   }

   // ajax method put /cards/task
   $.ajax({
      type: 'PUT',
      url: `https://localhost:5001/api/cards/task`,
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      dataType: 'json',
      data: JSON.stringify(req),
      success: function (response) {
         swal('Success!', 'Your task has been updated!', 'success')
         setTimeout(() => {
            location.reload()
         }, 1000)
      },
      error: function (e) {
         swal('Error!', `${JSON.parse(e.responseText).message}`, 'error')
      },
   })
}

const deleteTask = (taskId) => {
   swal({
      title: 'Are you sure?',
      text: 'Once deleted, you will not be able to recover this task!',
      icon: 'warning',
      buttons: true,
      dangerMode: true,
   }).then((willDelete) => {
      if (willDelete) {
         $.ajax({
            type: 'DELETE',
            url: `https://localhost:5001/api/checklistitems/delete/${taskId}`,
            success: function (response) {
               swal('Poof! Your task has been deleted!', {
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
         swal('Your task is safe!')
      }
   })
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

const updateCard = (userId) => {
   event.preventDefault()
   event.stopPropagation()

   /**
    * Id
    * ListId
    * Name
    * Description
    * CreatedBy
    * Due
    */
   const req = {
      Id: parseInt($('#card-id-card-update').val()),
      ListId: parseInt($('#list-id-card-update').val()),
      Name: $('#name-card-update').val(),
      Description: $('#description-card-update').val().replace(/\n/g, ' '),
      CreatedBy: userId,
      Due: $('#due-card-update').val(),
   }

   $.ajax({
      type: 'PUT',
      url: `https://localhost:5001/api/cards`,
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      dataType: 'json',
      data: JSON.stringify(req),
      success: function (response) {
         swal('Success!', 'Your card has been updated!', 'success')
         location.reload()
      },
      error: function (e) {
         swal('Error!', `${JSON.parse(e.responseText).message}`, 'error')
      },
   })
}

const deleteCard = () => {
   const cardId = $('#card-id').val()
   swal({
      title: 'Are you sure?',
      text: 'Once deleted, you will not be able to recover this card!',
      icon: 'warning',
      buttons: true,
      dangerMode: true,
   }).then((willDelete) => {
      if (willDelete) {
         $.ajax({
            type: 'DELETE',
            url: `https://localhost:5001/api/cards/${cardId}`,
            success: function (response) {
               swal('Poof! Your card has been deleted!', {
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
         swal('Your card is safe!')
      }
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

const createTask = () => {
   event.preventDefault()

   const req = {
      Name: $('#name-task').val(),
      Members: $('#members-task').val(),
      StartDate: $('#start-task').val(),
      Due: $('#due-task').val(),
      CardId: parseInt($('#card-id').val()),
   }

   console.log(req)

   if (req.Members.length === 0) {
      return swal('Oops!', 'You need to add at least one member!', 'error')
   }

   // // ajax method post /cards/task
   $.ajax({
      type: 'POST',
      url: `https://localhost:5001/api/cards/task`,
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      dataType: 'json',
      data: JSON.stringify(req),
      success: function (response) {
         swal('Success!', 'Your task has been created!', 'success')
         setTimeout(() => {
            location.reload()
         }, 1000)
      },
      error: function (e) {
         swal('Error!', `${JSON.parse(e.responseText).message}`, 'error')
      },
   })
}

const getListByBoardId = (id) => {}

const convertDateToInput = (date) => {
   let due = new Date(date)
   const year = due.getFullYear()
   const month = String(due.getMonth() + 1).padStart(2, '0')
   const day = String(due.getDate()).padStart(2, '0')
   const joined = [year, month, day].join('-')

   return joined
}

const sendComment = (userId) => {
   event.preventDefault()
   const req = {
      UserId: userId,
      CardId: parseInt($('#card-id').val()),
      Text: $('#comment-text').val(),
   }

   $.ajax({
      type: 'POST',
      url: `https://localhost:5001/api/comments`,
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      dataType: 'json',
      data: JSON.stringify(req),
      success: function (response) {
         $('#comment-text').val('')
         $('#commentModal').modal('hide')
         swal('Success!', 'Your comment has been sent!', 'success')
         location.reload()
      },
      error: function (e) {
         swal('Error!', `${JSON.parse(e.responseText).message}`, 'error')
      },
   })
}

const deleteComment = (commentId) => {
   event.preventDefault()
   // delete with alert swal
   swal({
      title: 'Are you sure?',
      text: 'Once deleted, you will not be able to recover this comment!',
      icon: 'warning',
      buttons: true,
      dangerMode: true,
   }).then((willDelete) => {
      if (willDelete) {
         $.ajax({
            type: 'DELETE',
            url: `https://localhost:5001/api/comments/${commentId}`,
            success: function (response) {
               swal('Poof! Your comment has been deleted!', {
                  icon: 'success',
               })
               setTimeout(() => {
                  location.reload()
               }, 1000)
            },
         })
      } else {
         swal('Your comment is safe!')
      }
   })
}
