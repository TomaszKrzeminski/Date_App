using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DateApp.Models
{

    public class ShowEventViewModel
    {
        public ShowEventViewModel()
        {
            list = new List<Event>();
            GetListFromDB = false;
            Name = "";
            Date = DateTime.Now.Date;
            DaysToEvent = 0;
            UserEvent = false;
            Distance = 0;
            CityNames = new List<string>();
            ZipCode = "";

        }

        public int Distance { get; set; }
        public string Name { get; set; }
        public int DaysToEvent { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public bool UserEvent { get; set; }
        public string UserId { get; set; }
        public List<Event> list { get; set; }
        public List<string> CityNames { get; set; }
        public bool GetListFromDB { get; set; }
        public string ZipCode { get; set; }
        //public List<string> ZipCodesForDistance { get; set; }

    }





    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        ShowEventViewModel Handle(ShowEventViewModel model);
    }


    abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;
        public ShowEventViewModel Model;
        public IRepository repo;
        public AbstractHandler(IRepository repository)
        {
            repo = repository;
        }


        public IHandler SetNext(IHandler handler)
        {
            this._nextHandler = handler;
            return handler;
        }

        public virtual ShowEventViewModel Handle(ShowEventViewModel model)
        {
            if (this._nextHandler != null)
            {
                return this._nextHandler.Handle(model);
            }
            else
            {
                return Model;
            }
        }
    }

    class NameHandler : AbstractHandler
    {


        public NameHandler(IRepository repository) : base(repository)
        {

        }


        public override ShowEventViewModel Handle(ShowEventViewModel model)
        {
            if (model.Name != null)
            {

                if (model.GetListFromDB == false)
                {
                    model.list = repo.GetEventsByName(model.Name);
                    model.GetListFromDB = true;
                    Model = model;

                }
                else
                {
                    model.list = model.list.Where(x => x.EventName == model.Name).ToList();
                    Model = model;
                }
            }
            else
            {
                Model = model;
            }
            return base.Handle(Model);

        }
    }

    class DateHandler : AbstractHandler
    {

        public DateHandler(IRepository repository) : base(repository)
        {

        }

        public override ShowEventViewModel Handle(ShowEventViewModel model)
        {
            if (model.Date != null)
            {

                if (model.GetListFromDB == false)
                {
                    model.list = repo.GetEventsByDate(model.Date);
                    model.GetListFromDB = true;
                    Model = model;
                }
                else
                {
                    model.list = model.list.Where(e => e.Date == model.Date).ToList();
                    Model = model;
                }

            }
            else
            {
                Model = model;
            }


            return base.Handle(Model);

        }
    }

    class UserHandler : AbstractHandler
    {

        public UserHandler(IRepository repository) : base(repository)
        {

        }

        public override ShowEventViewModel Handle(ShowEventViewModel model)
        {
            if (model.UserEvent)
            {

                if (model.GetListFromDB == false)
                {
                    model.list = repo.GetUserEvents(model.UserId);
                    model.GetListFromDB = true;

                }
                else
                {
                    model.list = model.list.Where(x => x.EventUser.First().AppUserId == model.UserId).ToList();

                }
                Model = model;

            }
            else
            {
                Model = model;
            }

            return base.Handle(Model);




        }
    }

    class CityNameHandler : AbstractHandler
    {

        public CityNameHandler(IRepository repository) : base(repository)
        {

        }



        public bool Check(List<string> list)
        {
            bool x = false;

            if (list != null)
            {
                foreach (var item in list)
                {
                    if (item != null)
                    {
                        x = true;
                    }
                }
            }
            return x;
        }


        public override ShowEventViewModel Handle(ShowEventViewModel model)
        {
            if (Check(model.CityNames))
            {

                if (model.GetListFromDB == false)
                {
                    model.list = repo.GetEventsByCities(model.CityNames);
                    model.GetListFromDB = true;

                }
                else
                {
                    List<Event> Events = new List<Event>();

                    foreach (var city in model.CityNames)
                    {
                        Events.AddRange(model.list.Where(x => x.City == city).ToList());
                    }

                    model.list = Events;

                }

                Model = model;
            }
            else
            {
                Model = model;
            }

            return base.Handle(Model);




        }
    }

    class ZipCodeHandler : AbstractHandler
    {

        public ZipCodeHandler(IRepository repository) : base(repository)
        {

        }


        public override ShowEventViewModel Handle(ShowEventViewModel model)
        {
            if (model.ZipCode != null && model.ZipCode != "")
            {

                if (model.GetListFromDB == false)
                {
                    List<string> ZipCode = new List<string>() { model.ZipCode };
                    model.list = repo.GetEventsByZipCodes(ZipCode);
                    model.GetListFromDB = true;
                }
                else
                {
                    model.list = model.list.Where(x => x.ZipCode == model.ZipCode).ToList();
                }

                Model = model;
            }
            else
            {
                Model = model;
            }

            return base.Handle(Model);




        }
    }

    class DistanceHandler : AbstractHandler
    {


        public DistanceHandler(IRepository repository) : base(repository)
        {

        }


        public List<string> CitiesInRange(string ZipCode = "86-100", int Distance = 10)
        {

            List<string> codes = new List<string>();
            try
            {

                string Key = "3fabbfd0-27e6-11eb-8826-59001fe1a22a";
                var httpClient1 = new HttpClient();

                var url = "https://app.zipcodebase.com/api/v1/radius?apikey=" + Key + "&code=" + ZipCode + "&radius=" + Distance + "&country=pl";
                HttpResponseMessage response1 = httpClient1.GetAsync(url).Result;
                string responseBody1 = response1.Content.ReadAsStringAsync().Result;
                JObject cityResponse = JObject.Parse(responseBody1);


                List<ZipDistanceDetails> list = cityResponse["results"].ToObject<List<ZipDistanceDetails>>();

                codes.AddRange(list.Select(c => c.code).ToList());
            }
            catch (Exception ex)
            {

            }

            codes = codes.Distinct().ToList();

            return codes;
        }



        public override ShowEventViewModel Handle(ShowEventViewModel model)
        {
            if (model.Distance != 0 && model.ZipCode != null && model.ZipCode != "")
            {
                List<string> ZipCodes = CitiesInRange(model.ZipCode, model.Distance);
                if (model.GetListFromDB == false)
                {

                    model.list = repo.GetEventsByZipCodes(ZipCodes);
                    model.GetListFromDB = true;

                }
                else
                {
                    //List<Event> ByZipCodes = new List<Event>();

                    //foreach (var ZipCode in ZipCodes)
                    //{
                    //    ByZipCodes.AddRange(model.list.Where(x => x.ZipCode == ZipCode).ToList());
                    //}

                    //model.list = ByZipCodes;

                    model.list = repo.GetEventsByZipCodes(ZipCodes);
                    model.GetListFromDB = true;

                }
                Model = model;

            }
            else
            {
                Model = model;
            }

            return base.Handle(Model);




        }






        //public override List<Event> Handle(ShowEventViewModel model)
        //{
        //    if (model.Distance > 0)
        //    {

        //        if (model.GetListFromDB == false)
        //        {
        //            model.list = repo.GetEventsByCities(model.CityNames);
        //            model.GetListFromDB = true;

        //        }
        //        else
        //        {
        //            List<Event> EventList = new List<Event>();

        //            foreach (var name in model.CityNames)
        //            {
        //                EventList.AddRange(model.list.Where(x => x.City == name).ToList());
        //            }

        //            model.list = EventList;
        //        }



        //        return model.list;
        //    }
        //    else
        //    {
        //        return model.list;
        //    }
        //}
    }
















}
