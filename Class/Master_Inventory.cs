using DevExpress.Utils.DirectXPaint;
using Pro_Salles.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Pro_Salles.Class.Master;

namespace Pro_Salles.Class
{
    class Master_Inventory
    {
        //From 66 to 70
        public enum CostCalculatingMethod
        {
            FIFO = 1,
            LIFO,
            WAVP
        }


        public static List<Value_And_ID> CostCalculatingMethod_List = new List<Value_And_ID>()
        {
            new Value_And_ID { ID = (int)CostCalculatingMethod.FIFO, Name = "الوارد اولا يخرج اولا" },
            new Value_And_ID { ID = (int)CostCalculatingMethod.LIFO, Name = "الوارد اخرآ يخرج اولا" },
            new Value_And_ID { ID = (int)CostCalculatingMethod.WAVP, Name = "المتوسط المرجح" },
        };

        public static double GetCostCalculatingMethod(int prodid, int storeid, double qty)
        {
            using (var db = new Pro_SallesDataContext())
            {
                var query = db.Store_Logs.Where(x => x.product_id == prodid && x.store_id == storeid).OrderBy(o => o.insert_time);
                if (query.Count() == 0)
                {
                    return Sessions.Product_View.Single(x => x.ID == prodid).Units.OrderByDescending(x => x.Factor).First().Buy_price;
                }
                double TotalQtyOut = query.Where(x => x.is_in_transaction == false).Sum(s => (double?)s.qty) ?? 0;
                double Balance = GetProductBalanceInStore(prodid, storeid);
                if (Balance <= 0)
                {
                    return 0;
                }
                var subQuery = query.Where(q => query.Where
                (q1 => q1.is_in_transaction == true && q1.insert_time <= q.insert_time).
                Sum(q1 => q1.qty) > TotalQtyOut && q.is_in_transaction == true).ToList();

                var subQueryBalance = subQuery.Where(x => x.is_in_transaction).Sum(x => x.qty) - subQuery.Where(s => s.is_in_transaction == false).Sum(s => s.qty);

                if (subQueryBalance > Balance && subQueryBalance != 0)/////////////////////////////
                {
                    //diff is the result between the minus operation of subquerybalance and Balance
                    var diff = subQueryBalance - Balance;

                    subQuery[0].qty -= diff;///////////////////Error
                }

                double FIFO;
                double LIFO;
                double WAC;

                if (subQuery[0].qty < qty)
                {
                    int i = 0;
                    var qtyx = qty;
                    double SumPrice = 0;
                    while (qtyx > 0 && i < subQuery.Count())
                    {
                        var row = subQuery[i];
                        double qty1 = (qtyx > row.qty) ? row.qty : qtyx;
                        SumPrice += qty1 * row.cost_value;
                        qtyx -= qty1;
                        i++;
                    }
                    FIFO = SumPrice / (qty - qtyx);
                }
                else
                {
                    FIFO = subQuery.First().cost_value;
                }

                subQuery = subQuery.OrderByDescending(q => q.insert_time).ToList();

                if (subQuery[0].qty < qty)
                {
                    int i = 0;
                    var qtyx = qty;
                    double SumPrice = 0;
                    while (qtyx > 0 && i < subQuery.Count())
                    {
                        var row = subQuery[i];
                        double qty1 = (qtyx > row.qty) ? row.qty : qtyx;
                        SumPrice += qty1 * row.cost_value;
                        qtyx -= qty1;
                        i++;
                    }
                    LIFO = SumPrice / (qty - qtyx); /////////////////////////
                }
                else
                {
                    LIFO = subQuery.First().cost_value;
                }


                WAC = subQuery.Select(q => q.qty * q.cost_value).Sum(q => q) / Balance;

                var costMethod = (CostCalculatingMethod)Sessions.Product.Single(x => x.ID == prodid).CostCalculatingMethod;

                switch (costMethod)
                {
                    case CostCalculatingMethod.FIFO:
                        return FIFO;
                    case CostCalculatingMethod.LIFO:
                        return LIFO;
                    case CostCalculatingMethod.WAVP:
                        return WAC;
                    default:
                        return FIFO;
                }
            }

        }
        public static double GetProductBalanceInStore(int prodid, int storeid)
        {
            using (var db = new Pro_SallesDataContext())
            {
                var query = db.Store_Logs.Where(x => x.product_id == prodid && x.store_id == storeid).OrderBy(o => o.insert_time);
                double TotalQtyOut = query.Where(x => x.is_in_transaction == false).Sum(s => (double?)s.qty) ?? 0;
                double TotalQtyIn = query.Where(x => x.is_in_transaction == true).Sum(s => (double?)s.qty) ?? 0;
                double Balance = TotalQtyIn - TotalQtyOut;
                return Balance;
            }
        }
    }
}