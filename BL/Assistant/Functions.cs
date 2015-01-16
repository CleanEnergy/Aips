using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityClasses;

namespace BL.Assistant
{
    public class Functions
    {
        private ApplicationDbContext context;
        private Validator validator;

        public Functions()
        {
            context = new ApplicationDbContext();
            validator = new Validator(context);
        }

        public async Task<ValidationResult> MarkLabPresence(string[] presentIdsArray, int labId)
        {
            ValidationResult validation = validator.MarkLabPresence(presentIdsArray, labId);

            if (validation)
            {
                Queries query = new Queries();
                string[] allSignedOnStudentIds = (await query.GetStudentsForLab(labId)).Select(x => x.Id).ToArray();

                using (DbContextTransaction t = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.ScheduledLabs.Add(new ScheduledLab() 
                        {
                            Date = DateTime.Today,
                            LabId = labId
                        });
                        await context.SaveChangesAsync();

                        int scheduledLabId = context.ScheduledLabs.Single(x => x.LabId == labId && x.Date == DateTime.Today).ScheduledLabId;

                        foreach (string id in allSignedOnStudentIds)
                        {
                            context.StudentsLabs.Add(new StudentsLabs()
                            {
                                StudentId = id,
                                WasPresent = presentIdsArray.Contains(id),
                                ScheduledLabId = scheduledLabId
                            });
                        }
                        await context.SaveChangesAsync();
                        t.Commit();
                    }
                    catch (Exception e)
                    {
                        t.Rollback();
                        throw e;
                    }
                }
            }

            return validation;
        }
    }
}
