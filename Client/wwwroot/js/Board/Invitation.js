const getInvitations = (userId) => {
   $.ajax({
      type: 'GET',
      url: `https://localhost:5001/api/verifyinvites/pending/${userId}`,
      dataType: 'json',
      success: function (response) {
         let html = ''
         response.data.map((item, i) => {
            html += ` <tr>
            <td>${i + 1}</td>
            <td>${item.pm}</td>
            <td>${item.boardName}</td>
            <td>${item.role}</td>
            <td>
               <button class="btn btn-primary mr-1" onclick="handleAccept(
               ${item.id}, ${item.userId}, ${true},'${item.role}', '${
               item.boardId
            }')">Accept</button>
               <button class="btn btn-outline-danger" onclick="handleReject(
                  ${item.id}, ${item.userId}, ${
               item.boardId
            },${false})">Reject</button>
            </td>
         </tr>
         `
         })

         if (html == '') {
            html += `
               <tr>
                  <td colspan="5" class="text-center">No Invitation Request</td>
               </tr>
               `
            $('#tbody-invitation').append(html)
         } else {
            $('#tbody-invitation').append(html)
         }
      },
      error: function (e) {
         console.log(e)
      },
   })
}

const handleAccept = (id, userId, isAccept, role, boardId) => {
   const req = {
      id,
      userId,
      isAccept,
      isUsed: true,
      role: role,
      boardId,
   }

   $.ajax({
      type: 'PUT',
      url: 'https://localhost:5001/api/verifyinvites/accept',
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      data: JSON.stringify(req),
      success: function (response) {
         swal('Horee! you have joined the board!', {
            icon: 'success',
         })
         location.reload()
      },
      error: function (e) {
         swal(`upps! ${JSON.parse(e.responseText).message}`, {
            icon: 'error',
         })
      },
   })
}

const handleReject = (id, userId, boardId) => {
   const req = {
      id,
      userId,
      boardId,
      isAccept: false,
      isUsed: true,
   }
   $.ajax({
      type: 'PUT',
      url: 'https://localhost:5001/api/verifyinvites/accept',
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      data: JSON.stringify(req),
      success: function (response) {
         swal('Horee! You have refused to join the board!', {
            icon: 'success',
         })

         setTimeout(() => {
            location.reload()
         }, 1000)
      },
      error: function (e) {
         swal(`upps! ${JSON.parse(e.responseText).message}`, {
            icon: 'error',
         })
      },
   })
}
