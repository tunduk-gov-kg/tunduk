create or replace function "GetMessagesCountGroupedByDateTime"("from" bigint, "to" bigint)
    returns TABLE
            (
                "DateTime"      timestamp with time zone,
                "MessagesCount" bigint
            )
    language plpgsql
as
$$
begin
    return query select TO_TIMESTAMP("ConsumerServerRequestInTs" / 1000) as "DateTime", count(*) as "MessagesCount"
                 from "Messages"
                 where ("ConsumerServerRequestInTs" >= "from")
                   and ("ConsumerServerRequestInTs" <= "to")
                   and ("ProducerServiceCode" not in (select * from "MetaServices"))
                 group by TO_TIMESTAMP("ConsumerServerRequestInTs" / 1000);
end;
$$;