// Get boards by user
$(document).ready(function () {
   const userId = $('#userId').val()
   $.LoadingOverlay('show')

   $.ajax({
      type: 'GET',
      contentType: 'application/json; charset=utf-8',
      dataType: 'json',
      url: `https://localhost:44308/api/boards/user/${userId}`,
      data: 'data',
      success: function (response) {
         $.LoadingOverlay('hide')

         let html = ''
         response.data.map((item) => {
            console.log(item)
            html += `
            <div class="col-md-6 col-lg-4 mb-4">
            <a href="/boards/detail/${item.id}" style="text-decoration: none;">
               <div class="box shadow rounded d-flex justify-content-between flex-column"
                  style="padding: 25px; min-height: 300px;">
                  <div class="top">
                     <div class="d-flex justify-content-between align-items-center mb-2">
                        <span class="badge badge-pill badge-info mb-3">Created at ${moment(
                           item.createdAt
                        ).format('LL')}</span>
                       ${
                          item.user.id == userId
                             ? `<div class='buttons d-flex' style='gap: 5px;'>
                              <div>
                              <button class='btn btn-outline-warning btn-sm' onclick="updateModal('${item.id}', ${userId}, '${item.name}', '${item.description}')">
                                   <i
                                      class='fa fa-pencil'
                                      aria-hidden='true'
                                   ></i>
                                </button>
                                </div>
                               <div>
                               <button class='btn btn-outline-danger btn-sm' onclick="deleteModal('${item.id}')">
                               <i
                                  class='fa fa-trash'
                                  aria-hidden='true'
                               ></i>
                            </button>
                            </div>
                             </div>`
                             : ''
                       }
                     </div>
                     <h4 style="color: black;">${item.name}</h4>
                     <p style="color: #646464;">
                        ${item.description}
                     </p>
                  </div>
      
                  <div class="bottom">
                     <div class="d-flex align-items-center justify-content-between">
                        <div class="pm" data-toggle="tooltip" data-placement="right"
                        title="${item.user.fullName} - PM">
                           <img src="https://ui-avatars.com/api/?name=${
                              item.user.fullName
                           }" alt="project-manager" width="40px"
                              class="rounded-circle">
                           <span class="pl-1" style="color: black;">${
                              item.user.fullName
                           }</span>
                        </div>
      
                        <div class="members d-flex pr-3">
                        ${item.memberBoards
                           .filter((member) => member.role !== 'PM')
                           .map((member) => {
                              return `
                              <img src="https://ui-avatars.com/api/?name=${member.user.fullName}&background=random" alt="${member.user.fullName}"
                              width="40px" class="rounded-circle" data-toggle="tooltip" data-placement="right"
                              title="${member.user.fullName} - ${member.role}" style="margin-right: -20px;">
                              `
                           })}
                        </div>
                     </div>
                  </div>
               </div>
            </a>
         </div>
            `
         })

         // const row = document.getElementById('row')

         $('#row').append(html)
      },
   })
})

const updateModal = (idBoard, idUser, name, description) => {
   event.stopPropagation()
   event.preventDefault()

   console.log(idBoard, idUser, name, description)

   $('#boardIdUpdate').val(idBoard)
   $('#userIdUpdate').val(idUser)
   $('#name_update').val(name)
   $('#description_update').val(description)

   $('#updateBoard').modal('show')
}

const updateBoard = () => {
   event.preventDefault()

   const id = $('#boardIdUpdate').val()
   const createdBy = $('#userIdUpdate').val()
   const name = $('#name_update').val()
   const description = $('#description_update').val().replace(/\n/g, ' ')

   const request = {
      id,
      name,
      description,
      createdBy,
   }

   $.ajax({
      type: 'PUT',
      contentType: 'application/json; charset=utf-8',
      dataType: 'json',
      url: 'https://localhost:44308/api/boards/',
      data: JSON.stringify(request),
      success: function (response) {
         location.reload()
      },
   })
}

const deleteModal = (id) => {
   event.stopPropagation()
   event.preventDefault()

   swal({
      title: 'Are you sure?',
      text: 'Once deleted, you will not be able to recover this board!',
      icon: 'warning',
      buttons: true,
      dangerMode: true,
   }).then((willDelete) => {
      if (willDelete) {
         $.ajax({
            type: 'DELETE',
            url: `https://localhost:44308/api/boards/${id}`,
            success: function (response) {
               swal('Poof! Your board has been deleted!', {
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
         swal('Your board is safe!')
      }
   })
}

const createBoard = () => {
   event.preventDefault()

   const userId = $('#userId').val()
   const name = $('#name').val()
   const description = $('#description').val().replace(/\n/g, ' ')

   $.ajax({
      type: 'POST',
      url: 'https://localhost:44308/api/boards/create',
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      data: JSON.stringify({
         Name: name,
         Description: description,
         CreatedBy: userId,
      }),
      dataType: 'json',
   })
      .done((response) => {
         swal('Success!', 'Your board has been created!', 'success')
         $('#createBoard').modal('hide')

         $(document).ajaxStop(function () {
            window.location.reload()
         })
      })
      .fail((e) => {
         swal('Error!', `${JSON.parse(e.responseText).message}`, 'error')
      })

   console.log('Create Board', userId, name, description)
}
