const getMembersBoardById = (id, userId) => {
   // request ajax method: GET, url: https://localhost:44372/api/Board/GetMembersBoardById/{id}
   // return ajax method: GET, url: https://localhost:44372/api/Board/GetMembersBoardById/{id}
   return $.ajax({
      type: 'GET',
      url: `https://localhost:44308/api/memberboards/board/${id}`,
      contentType: 'application/json; charset=utf-8',
      dataType: 'json',
      success: function (response) {
         let html = ''
         response.data.map((item, i) => {
            console.log(item)
            html += `
            <tr>
               <td>${i + 1}</td>
               <td>${item.user.fullName}</td>
               <td>${item.user.email}</td>
               <td>${item.role}</td>
               <td>
               ${
                  item.board.createdBy == userId
                     ? item.role != 'PM'
                        ? `<button class="btn btn-warning mr-2" onclick="openUpdateMemberModal(${item.id}, ${item.user.id}, '${item.boardId}', '${item.user.email}', '${item.role}')">
                     <i class="fa fa-pencil" aria-hidden="true"></i>
                  </button>
                  <button class="btn btn-outline-danger" onclick="kickMember(${item.id})">
                     kick
                  </button>
                  `
                        : `
                        <button class="btn btn-secondary" disabled>DISABLED</button>
                        `
                     : item.user.id == userId
                     ? `<button class="btn btn-outline-danger" onclick="leaveMember(${item.id})">Leave</button>`
                     : `<button class="btn btn-secondary" disabled>DISABLED</button>`
               }
               </td>
            </tr>
            `
         })

         $('#members').append(html)
      },
      error: function (e) {
         location.href = '/boards'
      },
   })
}

const kickMember = (id) => {
   swal({
      title: 'Are you sure?',
      text: 'Once deleted, you will not be able to recover!',
      icon: 'warning',
      buttons: true,
      dangerMode: true,
   }).then((willDelete) => {
      if (willDelete) {
         $.ajax({
            type: 'DELETE',
            url: `https://localhost:44308/api/memberboards/delete/${id}`,
            success: function (response) {
               location.reload()
            },
         })
         swal('Poof! Your member has been deleted!', {
            icon: 'success',
         })
      } else {
         swal('Your member is safe!')
      }
   })
}

const leaveMember = (id) => {
   swal({
      title: 'Are you sure?',
      text: 'Once deleted, you will not be able to recover!',
      icon: 'warning',
      buttons: true,
      dangerMode: true,
   }).then((willDelete) => {
      if (willDelete) {
         $.ajax({
            type: 'DELETE',
            url: `https://localhost:44308/api/memberboards/delete/${id}`,
            success: function (response) {
               swal('Poof! Your member has been deleted!', {
                  icon: 'success',
               })

               setTimeout(() => {
                  location.href = '/boards'
               }, 1200)
            },
         })
      } else {
         swal('Your member is safe!')
      }
   })
}

const openUpdateMemberModal = (id, userId, boardId, email, role) => {
   console.log(id, userId, boardId, email, role)

   $('#memberId').val(id)
   $('#userId').val(userId)
   $('#boardId').val(boardId)
   $('#email').val(email)
   $('#role').val(role)

   $('#updateMemberModal').modal('show')
}

const updateMember = () => {
   event.preventDefault()

   let id = parseInt($('#memberId').val())
   let userId = parseInt($('#userId').val())
   let boardId = $('#boardId').val()
   let email = $('#email').val()
   let role = $('#role').val()
   console.log(id, userId, boardId, email, role)

   $.ajax({
      type: 'PUT',
      url: `https://localhost:44308/api/memberboards`,
      data: JSON.stringify({
         id,
         userId: userId,
         boardId: boardId,
         role: role,
      }),
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      dataType: 'json',
      success: function (response) {
         swal('Success!', 'Your member has been updated!', 'success')

         setTimeout(() => {
            location.reload()
         }, 1000)
      },
      error: function (e) {
         swal('Error!', `${JSON.parse(e.responseText).message}`, 'error')
      },
   })

   console.log(userId, boardId, email, role)
}
