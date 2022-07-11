

using Grpc.Net.Client;

namespace StudentsClient;

class Program
    {
        static async Task Main(string[] args)
        { 

            var channel = GrpcChannel.ForAddress("https://localhost:7003");
            
            string exit;
            do
            {
             Console.WriteLine("As of today, there are these people on staff:");
            await displayAllStudents(channel);
            Console.WriteLine("Choose a next action:");
            Console.WriteLine("Add new person - enter  (NEW)");
            Console.WriteLine("Remove person from database - enter (DEL)");
          //  Console.WriteLine("Change the data of the person in the database - type  (EDIT)");
            string ans = Console.ReadLine();
            switch (ans)
            {
                case "NEW":
                    Console.WriteLine("Adding new person:");
                    Console.WriteLine("FirstName:");
                    string n_fn = Console.ReadLine();
                    Console.WriteLine("LastName:");
                    string n_ln = Console.ReadLine();
                    Console.WriteLine("Email:");
                    string n_em = Console.ReadLine();
                    
                    StudentModel newStudent = new StudentModel()
                    {
                        FirstName = n_fn,
                        LastName = n_ln,
                        Email = n_em,
                    };
                    await insertStudent(channel, newStudent);
                    break;
                
                case "DEL":
                    Console.WriteLine("Removing a person with an ID?:");
                    int d_id = Convert.ToInt32(Console.ReadLine());
                    await deleteStudent(channel, d_id);
                    break;
                case "EDIT":
                    Console.WriteLine("Changing a person's data:");
                    Console.WriteLine("StudentId:");
                    int e_id = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("FirstName:");
                    string e_fn = Console.ReadLine();
                    Console.WriteLine("LastName:");
                    string e_ln = Console.ReadLine();
                    Console.WriteLine("Email:");
                    string e_em = Console.ReadLine();
                    
                    StudentModel updStudent = new StudentModel()
                    {
                        StudentId = e_id,
                        FirstName = e_fn,
                        LastName = e_ln,
                        Email = e_em,
                    };
                    await updateStudent(channel, updStudent);
                    break;
            }
            
            Console.WriteLine("Leaving the Journal? Y/N");
            exit = Console.ReadLine();
            }
            while (exit == "N");
            Console.ReadLine();
        }

        static async Task findStudentById(GrpcChannel channel, int id)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var input = new StudentLookupModel { StudentId = id };
            var reply = await client.GetStudentInfoAsync(input);
            Console.WriteLine($"{reply.FirstName} {reply.LastName}");
        }

        static async Task insertStudent(GrpcChannel channel, StudentModel student)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var reply = await client.InsertStudentAsync(student);
            Console.WriteLine(reply.Result);
        }

        static async Task updateStudent(GrpcChannel channel, StudentModel student)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var reply = await client.UpdateStudentAsync(student);
            Console.WriteLine(reply.Result);
        }

        static async Task deleteStudent(GrpcChannel channel, int id)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var input = new StudentLookupModel { StudentId = id };
            var reply = await client.DeleteStudentAsync(input);
            Console.WriteLine(reply.Result);
        }

        static async Task displayAllStudents(GrpcChannel channel)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var empty = new Empty();
            var list = await client.RetrieveAllStudentsAsync(empty);

            Console.WriteLine(">>>>>>>>>>>>>>>>>>++++++++++++<<<<<<<<<<<<<<<<<<<<<<<<<<<<");

            foreach (var item in list.Items)
            {
                Console.WriteLine($"{item.StudentId}: {item.FirstName} {item.LastName}");
            }
            Console.WriteLine(">>>>>>>>>>>>>>>>>>++++++++++++<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
       }


    }