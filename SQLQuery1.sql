﻿SELECT UserTable.UserId,UserTable.Name,UserTable.Surname,UserTable.Email,UserTable.Phonenumber,UserTable.UserType,AddressList.StreetName,AddressList.Streetnr,PostalCode.City,PostalCode.PostalCode FROM UserTable
                                    INNER JOIN AddressList ON UserTable.UserId=AddressList.UserId INNER join PostalCode on AddressList.PostalCode=PostalCode.PostalCode
                                    WHERE UserTable.Email = 'andreas@hotmail.com' and UserTable.Password = 12345678;