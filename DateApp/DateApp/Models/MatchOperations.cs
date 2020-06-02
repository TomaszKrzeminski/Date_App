using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models
{

    public class MatchDetails
    {
        public int Age;
        public int Distance;
        public string Sex;


        public MatchDetails(int Age,int Distance,string Sex)
        {
            this.Age = Age;
            this.Distance = Distance;
            this.Sex = Sex;
        }
    }

    public class UserDetails:MatchDetails
    {
        public UserDetails(int Age,int Distance,string Sex):base(Age,Distance,Sex)
        {

        }
    }



    public class MatchSearch
    {

        protected MatchSearch matchSearch;
        public List<Coordinates> list;
        public Coordinates coordinates;
       

        public MatchSearch(Coordinates c,List<Coordinates> list)
        {
            this.coordinates = c;
            this.list = list;
           
        }

        public void setMatch(MatchSearch matchSearch)
        {
            this.matchSearch = matchSearch;
        }

        public virtual void ForwardRequest(MatchDetails MatchDetails,UserDetails userDetails)
        {

        }


    }

    public class  DistanceMatch:MatchSearch
    {
        public DistanceMatch(Coordinates coordinates,List<Coordinates> list) :base(coordinates,list)
        {
           
        }


        public override void ForwardRequest(MatchDetails details, UserDetails userDetails)
        {
            if (details.Distance>=userDetails.Distance)
            {
                list.Add(coordinates);
            }
            else if (matchSearch != null)
            {
                matchSearch.ForwardRequest(details,userDetails);
            }
        }


    }

    public class AgeMatch : MatchSearch
    {
        public AgeMatch(Coordinates coordinates, List<Coordinates> list) : base(coordinates, list)
        {

        }

        public override void ForwardRequest(MatchDetails details, UserDetails userDetails)
        {
            if (details.Age <= userDetails.Age)
            {
                matchSearch.ForwardRequest(details, userDetails);
            }
            else if (matchSearch != null)
            {
                
            }
        }


    }

    public class SexMatch : MatchSearch
    {
        public SexMatch(Coordinates coordinates, List<Coordinates> list) : base(coordinates, list)
        {

        }

        public override void ForwardRequest(MatchDetails details, UserDetails userDetails)
        {
            if (details.Sex != userDetails.Sex)
            {
                 matchSearch.ForwardRequest(details, userDetails);
            }
            else if(matchSearch!=null)
            {
               
            }
        }


    }










}
